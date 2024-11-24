using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class NightManager : Frame.SceneManager {
    public static NightManager Instance { get; private set; }
    [FormerlySerializedAs("m_NightMapChanceSheet")] [FormerlySerializedAs("mNightMapChanceSheet")] [FormerlySerializedAs("m_mapChanceSheet")] [SerializeField] private NightMapChanceSheet m_nightMapChanceSheet;
    /// <summary>
    ///  [i, j]，i是行，j是列
    /// </summary>
    private NightMapStateEnum[,] m_map;
    private bool[,] m_shadow;
    [SerializeField] private Transform m_mapTransform;
    [SerializeField] private Transform m_shadowTransform;
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
        m_nightMapChanceSheet.Check();
        m_map = new NightMapStateEnum[m_nightMapChanceSheet.MapCount, m_nightMapChanceSheet.MapCount];
        m_shadow = new bool[m_nightMapChanceSheet.MapCount, m_nightMapChanceSheet.MapCount];
        List<Vector2Int> paths = new();
        NightMapStateEnum[] sample = new NightMapStateEnum[] { NightMapStateEnum.CHEST, NightMapStateEnum.PROP, NightMapStateEnum.PATH, NightMapStateEnum.MONSTER };
        for (int i = 0; i < m_nightMapChanceSheet.MapCount; i++) {
            for (int j = 0; j < m_nightMapChanceSheet.MapCount; j++) {
                m_shadow[i, j] = true;
                InstantiateTile(m_shadowPrefab , i,j, m_shadowTransform);
                m_map[i, j] = m_nightMapChanceSheet.GetRandomValue(sample);
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
        Player = InstantiateTile(m_playerPrefab, pos.x, pos.y, transform).GetComponent<NightPlayer>();
        Player.Position = pos;
        ShowMap(pos);
    }
    private void HandleMove()  {
        Vector2 pos = MainScene.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int index = new Vector2Int(-HandlePointToArrayIndex(pos.y), HandlePointToArrayIndex(pos.x));
        if (!InMap(index.x, index.y)) {
            // Debug.Log(string.Format("OUT OF THE MAP {0}", index));
            return;
        }
        if (m_shadow[index.x, index.y]) {
            // Debug.Log(string.Format("IS SHADOW {0}", index));
            return;
        }
        if (index == Player.Position) {
            return;
        }
        NightMapStateEnum target = m_map[index.x, index.y];
        List<Vector2Int> paths = GoTo(index);
        Debug.Log(paths.Count);
        // Debug.Log(paths.Count);
        return;
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
    private List<Vector2Int> GoTo(Vector2Int target) {
        Debug.Log("Begin to walk");
        int N = m_nightMapChanceSheet.MapCount;
        bool[] visited = new bool[N * N];
        void MarkVisited(Vector2Int pos) {
            visited[pos.x * N + pos.y] = true;
        }
        bool CheckVisited(Vector2Int pos) {
            return !visited[pos.x * N + pos.y];
        }
        Queue<List<Vector2Int>> path = new();
        List<Vector2Int> t = new();
        t.Add(Player.Position);
        path.Enqueue(t);
        int times = 0;
        while (path.Count > 0) {
            if (times++ >= 100) {
                // Debug.Log(string.Format("CAN NOT GO {0}", target))
                return new List<Vector2Int>();
            }
            List<Vector2Int> currList = path.Dequeue();
            Vector2Int current = currList[^1];
            if (current == target) {
                return currList;    
            }
            MarkVisited(current);
            Vector2Int left = new Vector2Int(current.x - 1, current.y);
            if (CanGo(left) && CheckVisited(left)) {
                List<Vector2Int> to_add = new(currList);
                to_add.Add(new Vector2Int(left.x, left.y));
                path.Enqueue(to_add);
            }
            Vector2Int right = new Vector2Int(current.x + 1, current.y);
            if (CanGo(right) && CheckVisited(right)) {
                List<Vector2Int> to_add = new(currList);
                to_add.Add(new Vector2Int(right.x, right.y));
                path.Enqueue(to_add);
            }
            Vector2Int up = new Vector2Int(current.x, current.y - 1);
            if (CanGo(up) && CheckVisited(up)) {
                List<Vector2Int> to_add = new(currList);
                to_add.Add(new Vector2Int(up.x, up.y));
                path.Enqueue(to_add);
            }
            Vector2Int down = new Vector2Int(current.x, current.y + 1);
            if (CanGo(down) && CheckVisited(down)) {
                List<Vector2Int> to_add = new(currList);
                to_add.Add(new Vector2Int(down.x, down.y));
                path.Enqueue(to_add);
            }
        }
        return new List<Vector2Int>();
    }
    bool CanGo(Vector2Int pos) {
        int x = pos.x;
        int y = pos.y;
        return InMap(x, y) && !m_shadow[x, y];   
    }
    private int HandlePointToArrayIndex(float p) {
        return Mathf.RoundToInt(p);
    }
    public bool InMap(int x, int y) {
        return x >= 0 && y >= 0  && x < m_nightMapChanceSheet.MapCount && y < m_nightMapChanceSheet.MapCount;
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
        Transform shadow = m_shadowTransform.GetChild(r * m_nightMapChanceSheet.MapCount + c);
        shadow.gameObject.SetActive(false);
    }
}
