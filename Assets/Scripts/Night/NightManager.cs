using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NightManager : SceneManager {
    public static NightManager Instance { get; private set; }
    [FormerlySerializedAs("mNightMapChanceSheet")] [FormerlySerializedAs("m_mapChanceSheet")] [SerializeField] private NightMapChanceSheet m_NightMapChanceSheet;
    /// <summary>
    ///  [i, j]，i是行，j是列
    /// </summary>
    private NightMapStateEnum[,] m_map;
    private bool[,] m_shadow;
    [SerializeField] private Transform m_mapTransform;
    // private static readonly string CHEST_PATH = "Scenes/Night/Prefabs/Chest";
    // private static readonly string PROP_PATH = "";
    // private static readonly string PATH_PATH = "";
    // private static readonly string MONSTER_PATH = "";
    [SerializeField] private GameObject m_chestPrefab;
    [SerializeField] private GameObject m_propPrefab;
    [SerializeField] private GameObject m_pathPrefab;
    [SerializeField] private GameObject m_monsterPrefab;
    [SerializeField] private GameObject m_playerPrefab;
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
            Camera.main.transform.position = pos;
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
                m_map[i, j] = m_NightMapChanceSheet.GetRandomValue(sample);
                switch (m_map[i, j]) {
                    case NightMapStateEnum.CHEST:
                        InstantiateTile(m_chestPrefab, i, j);
                        break;
                    case NightMapStateEnum.PROP:
                        InstantiateTile(m_propPrefab, i, j);
                        break;
                    case NightMapStateEnum.PATH:
                        paths.Add(new Vector2Int(i, j));
                        InstantiateTile(m_pathPrefab, i, j);
                        break;
                    case NightMapStateEnum.MONSTER:
                        InstantiateTile(m_monsterPrefab, i, j);
                        break;
                }
            }
        }
        InstantiatePlayer(paths[Random.Range(0, paths.Count)]);
    }
    private GameObject InstantiateTile(GameObject go, int r, int c) {
        GameObject instance = Instantiate(go, m_mapTransform);
        instance.transform.position = new Vector2(c, -r);   // Unity 和 计算机图形学的坐标系不一样，:(
        instance.GetComponent<NightMapObject>().Position = new Vector2Int(r, c);
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
        if (!InMap(index)) {
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
    public bool InMap(Vector2Int pos) {
        return pos.x >= 0 && pos.y >= 0 && pos.x < m_NightMapChanceSheet.MapCount && pos.y < m_NightMapChanceSheet.MapCount;
    }
    public void ShowMap(Vector2Int pos) {
        m_shadow[pos.x, pos.y] = false;
        if (InMap(new Vector2Int(pos.x - 1, pos.y))) {
            m_shadow[pos.x - 1, pos.y] = false;
        }
        if (InMap(new Vector2Int(pos.x + 1, pos.y))) {
            m_shadow[pos.x + 1, pos.y] = false;
        }
        if (InMap(new Vector2Int(pos.x, pos.y - 1))) {
            m_shadow[pos.x, pos.y - 1] = false;
        }
        if (InMap(new Vector2Int(pos.x, pos.y + 1))) {
            m_shadow[pos.x, pos.y + 1] = false;
        }
        if (InMap(new Vector2Int(pos.x - 1, pos.y - 1))) {
            m_shadow[pos.x - 1, pos.y - 1] = false;
        }
        if (InMap(new Vector2Int(pos.x - 1, pos.y + 1))) {
            m_shadow[pos.x - 1, pos.y + 1] = false;
        }
        if (InMap(new Vector2Int(pos.x + 1, pos.y - 1))) {
            m_shadow[pos.x + 1, pos.y - 1] = false;
        }
        if (InMap(new Vector2Int(pos.x + 1, pos.y + 1))) {
            m_shadow[pos.x + 1, pos.y + 1] = false;
        }
    }
}
