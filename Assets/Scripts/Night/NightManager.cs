using System.Collections.Generic;
using UnityEngine;
public class NightManager : SceneManager {
    public static NightManager Instance { get; private set; }
    [SerializeField] private MapChanceSheet m_mapChanceSheet;
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
    public GameObject Player { get; private set; }
    protected override void Awake() {
        base.Awake();
        Instance = this;
        GenerateMap();
    }
    private void Update() {
        HandleMove();
        if (Input.GetMouseButtonUp(0)) {
            // HandleMove();
        }
        if (Player) {
            Vector3 pos = Player.transform.position;
            pos.z = Camera.main.transform.position.z;
            Camera.main.transform.position = pos;
        }
    }
    private void GenerateMap() {
        m_mapChanceSheet.Check();
        m_map = new NightMapStateEnum[m_mapChanceSheet.MapCount, m_mapChanceSheet.MapCount];
        List<Vector2Int> paths = new();
        // List<NightMapStateEnum> temp = new();
        NightMapStateEnum[] sample = new NightMapStateEnum[4] { NightMapStateEnum.CHEST, NightMapStateEnum.PROP, NightMapStateEnum.PATH, NightMapStateEnum.MONSTER };
        for (int i = 0; i < m_mapChanceSheet.MapCount; i++) {
            for (int j = 0; j < m_mapChanceSheet.MapCount; j++) {
                m_map[i, j] = m_mapChanceSheet.GetRandomValue(sample);
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
        instance.GetComponent<NightMapObject>().Position = new Vector2(r, c);
        return instance;
    }
    private void InstantiatePlayer(Vector2Int pos) {
        Player = InstantiateTile(m_playerPrefab, pos.x, pos.y);
    }
    private void HandleMove() {
        
    }
}
