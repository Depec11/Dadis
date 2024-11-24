using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NightManager : SceneManager {
    public static NightManager Instance { get; private set; }
    [FormerlySerializedAs("m_mapChanceSheet")] [SerializeField] private NightMapChanceSheet mNightMapChanceSheet;
    /// <summary>
    ///  [i, j]，i是行，j是列
    /// </summary>
    private NightMapStateEnum[,] m_map;
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
        mNightMapChanceSheet.Check();
        m_map = new NightMapStateEnum[mNightMapChanceSheet.MapCount, mNightMapChanceSheet.MapCount];
        List<Vector2Int> paths = new();
        // List<NightMapStateEnum> temp = new();
        NightMapStateEnum[] sample = new NightMapStateEnum[4] { NightMapStateEnum.CHEST, NightMapStateEnum.PROP, NightMapStateEnum.PATH, NightMapStateEnum.MONSTER };
        for (int i = 0; i < mNightMapChanceSheet.MapCount; i++) {
            for (int j = 0; j < mNightMapChanceSheet.MapCount; j++) {
                m_map[i, j] = mNightMapChanceSheet.GetRandomValue(sample);
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
    }
    private void HandleMove()  {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int r = -HandlePointToArrayIndex(pos.y);
        int c = HandlePointToArrayIndex(pos.x);
        if (r < 0 || c < 0 || r >= mNightMapChanceSheet.MapCount || c >= mNightMapChanceSheet.MapCount) {
            Debug.Log("RETURN ON OUT OF MAP");
            return;
        }
        if (r == Player.Position.x && r == Player.Position.y) {
            Debug.Log("RETURN ON AS PLAYER");
            return;
        }
        Debug.Log(new Vector2Int(r, c));
        Debug.Log(Player.Position);
        NightMapStateEnum target = m_map[r, c];
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
}
