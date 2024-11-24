using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class NightManager : Frame.SceneManager {
    public static NightManager Instance { get; private set; }
    [FormerlySerializedAs("mNightMapChanceSheet")] [FormerlySerializedAs("m_mapChanceSheet")] [SerializeField] private NightMapChanceSheet m_NightMapChanceSheet;
    /// <summary>
    ///  [i, j]，i是行，j是列
    /// </summary>
    private NightMapStateEnum[,] m_map;
    private bool[,] m_shadow;
    [SerializeField] private Transform m_mapTransform;
    [SerializeField] private Transform m_shadowTransform;
    // private static readonly string CHEST_PATH = "Scenes/Night/Prefabs/Chest";
    // private static readonly string PROP_PATH = "";
    // private static readonly string PATH_PATH = "";
    // private static readonly string MONSTER_PATH = "";
    [SerializeField] private GameObject m_chestPrefab;
    [SerializeField] private GameObject m_propPrefab;
    [SerializeField] private GameObject m_pathPrefab;
    [SerializeField] private GameObject m_monsterPrefab;
    [SerializeField] private GameObject m_playerPrefab;
    [SerializeField] private GameObject m_shadowPrefab;
    public NightPlayer Player { get; private set; }
    protected override void Awake() {
        base.Awake();
        Instance = this;
        GenerateMap();
    }
    public override void Load()  {
        throw new System.NotImplementedException();
    }
    private void Update() {
        if (Input.GetMouseButtonUp(0)) {
            HandleMove();
        }
        if (Player) {
            Vector3 pos = Player.transform.position;
            pos.z = Camera.main.transform.position.z;
            MainScene.MainCamera.transform.position = pos;
        }
    }
    private void GenerateMap() {
        m_NightMapChanceSheet.Check();
        m_map = new NightMapStateEnum[m_NightMapChanceSheet.MapCount, m_NightMapChanceSheet.MapCount];
        m_shadow = new bool[m_NightMapChanceSheet.MapCount, m_NightMapChanceSheet.MapCount];
        List<Vector2Int> paths = new();
        // List<NightMapStateEnum> temp = new();
        NightMapStateEnum[] sample = new NightMapStateEnum[] { NightMapStateEnum.CHEST, NightMapStateEnum.PROP, NightMapStateEnum.PATH, NightMapStateEnum.MONSTER };
        for (int i = 0; i < m_NightMapChanceSheet.MapCount; i++) {
            for (int j = 0; j < m_NightMapChanceSheet.MapCount; j++) {
                m_shadow[i, j] = true;
                InstantiateTile(m_shadowPrefab , i,j, m_shadowTransform);
                m_map[i, j] = m_NightMapChanceSheet.GetRandomValue(sample);
                switch (m_map[i, j]) {
                    case NightMapStateEnum.CHEST:
                        InstantiateTile(m_chestPrefab, i, j, m_mapTransform);
                        break;
                    case NightMapStateEnum.PROP:
                        InstantiateTile(m_propPrefab, i, j, m_mapTransform);
                        break;
                    case NightMapStateEnum.PATH:
                        paths.Add(new Vector2Int(i, j));
                        InstantiateTile(m_pathPrefab, i, j, m_mapTransform);
                        break;
                    case NightMapStateEnum.MONSTER:
                        InstantiateTile(m_monsterPrefab, i, j, m_mapTransform);
                        break;
                }
            }
        }
        InstantiatePlayer(paths[Random.Range(0, paths.Count)]);
    }
    private GameObject InstantiateTile(GameObject go, int r, int c, Transform parent = null) {
        GameObject instance = Instantiate(go, parent);
        instance.transform.localPosition = new Vector2(c, -r);   // Unity 和 计算机图形学的坐标系不一样，:(
        NightMapObject nmo = instance.GetComponent<NightMapObject>();
        if (nmo) {
            nmo.Position = new Vector2Int(r, c);
        }
        return instance;
    }
    private void InstantiatePlayer(Vector2Int pos) {
        Player = InstantiateTile(m_playerPrefab, pos.x, pos.y).GetComponent<NightPlayer>();
        Player.Position = pos;
        ShowMap(pos);
    }
    private void HandleMove()  {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int index = new Vector2Int(-HandlePointToArrayIndex(pos.y), HandlePointToArrayIndex(pos.x));
        if (!InMap(index.x, index.y)) {
            return;
        }
        if (m_shadow[index.x, index.y]) {
            return;
        }
        if (index == Player.Position) {
            return;
        }
        NightMapStateEnum target = m_map[index.x, index.y];
        switch (target) {
            case NightMapStateEnum.CHEST: 
                Debug.Log("OPEN CHEST"); 
                break;
            case NightMapStateEnum.PROP: 
                Debug.Log("PICK UP THE PROP"); 
                break;
            case NightMapStateEnum.PATH: 
                Debug.Log("GO TO THE PATH"); 
                break;
            case NightMapStateEnum.MONSTER: 
                Debug.Log("YOU SHOULD DEFEAT THE MONSTER"); 
                break;
        }
    }
    private int HandlePointToArrayIndex(float p) {
        return Mathf.RoundToInt(p);
    }
    public bool InMap(int x, int y) {
        return x >= 0 && y >= 0  && x < m_NightMapChanceSheet.MapCount && y < m_NightMapChanceSheet.MapCount;
    }
    public void ShowMap(Vector2Int pos) {
        ShowShadow(pos.x, pos.y);
        if (InMap(pos.x - 1, pos.y)) {
            ShowShadow(pos.x - 1, pos.y);
        }
        if (InMap(pos.x + 1, pos.y)) {
            ShowShadow(pos.x + 1, pos.y);
        }
        if (InMap(pos.x, pos.y - 1)) {
            ShowShadow(pos.x, pos.y - 1);
        }
        if (InMap(pos.x, pos.y + 1)) {
            ShowShadow(pos.x, pos.y + 1);
        }
        if (InMap(pos.x - 1, pos.y - 1)) {
            ShowShadow(pos.x - 1, pos.y - 1);
        }
        if (InMap(pos.x - 1, pos.y + 1)) {
            ShowShadow(pos.x - 1, pos.y + 1);
        }
        if (InMap(pos.x + 1, pos.y - 1)) {
            ShowShadow(pos.x + 1, pos.y - 1);
        }
        if (InMap(pos.x + 1, pos.y + 1)) {
            ShowShadow(pos.x + 1, pos.y + 1);
        }
    }
    private void ShowShadow(int r, int c) {
        m_shadow[r, c] = false;
        Transform shadow = m_shadowTransform.GetChild(r * m_NightMapChanceSheet.MapCount + c + 1);
        shadow.gameObject.SetActive(false);
    }
}
