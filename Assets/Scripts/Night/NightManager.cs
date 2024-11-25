using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class NightManager : Frame.SceneManager {
    public static NightManager Instance { get; private set; }
    [SerializeField] private NightMapChanceSheet m_nightMapChanceSheet;
    [SerializeField] private NightMonsterChanceSheet m_NightMonsterChanceSheet;
    [SerializeField] private NightChestChanceSheet m_NightChestChanceSheet;
    [SerializeField] private NightPropChanceSheet m_NightPropChanceSheet;
    /// <summary>
    ///  [i, j]，i是行，j是列
    /// </summary>
    private NightMapStateEnum[,] m_map;
    private bool[,] m_shadow;
    [SerializeField] private Transform m_mapParent;
    [SerializeField] private Transform m_shadowParent;
    [SerializeField] private Transform m_playerParent;
    [SerializeField] private GameObject m_chestPrefab;
    [SerializeField] private GameObject m_propPrefab;
    [SerializeField] private GameObject m_pathPrefab;
    [SerializeField] private GameObject m_monsterPrefab;
    [SerializeField] private GameObject m_playerPrefab;
    [SerializeField] private GameObject m_shadowPrefab;
    private List<Vector2Int> m_paths = new();
    public NightPlayerObject Player { get; private set; }
    [SerializeField] private float m_playerSpeed = 10.0f;
    protected override void Awake() {
        base.Awake();
        Instance = this;
        GenerateMap();
    }
    public override void Load()  {
        throw new System.NotImplementedException();
    }
    private void Update() {
        if (m_paths.Count == 0 && Input.GetMouseButtonUp(0)) {
            HandleMove();
        } else if (m_paths.Count > 0) {
            Move();
        }
        if (Player) {
            Vector3 pos = Player.transform.position;
            Player.transform.localPosition = new Vector3(pos.x, pos.y, 0);
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
                InstantiateTile(m_shadowPrefab , i,j, parent:m_shadowParent);
                m_map[i, j] = m_nightMapChanceSheet.GetRandomValue(sample);
                switch (m_map[i, j]) {
                    case NightMapStateEnum.CHEST:
                        InstantiateTile(m_chestPrefab, i, j, NightMapStateEnum.CHEST, m_mapParent);
                        break;
                    case NightMapStateEnum.PROP:
                        InstantiateTile(m_propPrefab, i, j, NightMapStateEnum.PROP, m_mapParent);
                        break;
                    case NightMapStateEnum.PATH:
                        paths.Add(new Vector2Int(i, j));
                        InstantiateTile(m_pathPrefab, i, j, parent:m_mapParent);
                        break;
                    case NightMapStateEnum.MONSTER:
                        InstantiateTile(m_monsterPrefab, i, j, NightMapStateEnum.MONSTER, m_mapParent);
                        break;
                }
            }
        }
        InstantiatePlayer(paths[Random.Range(0, paths.Count)]);
    }
    private GameObject InstantiateTile(GameObject go, int r, int c, NightMapStateEnum type = NightMapStateEnum.SHOWED, Transform parent = null) {
        GameObject instance = Instantiate(go, parent);
        instance.transform.localPosition = new Vector2(c, -r);   // Unity 和 计算机图形学的坐标系不一样，:(
        NightMapObject nmo = instance.GetComponent<NightMapObject>();
        if (nmo) {
            nmo.Position = new Vector2Int(r, c);
        }

        switch (type) {
            case NightMapStateEnum.CHEST:
                ((NightChest)nmo).data = m_NightChestChanceSheet.RandomValue();
                break;
            case NightMapStateEnum.PROP: 
                ((NightProp)nmo).data = m_NightPropChanceSheet.RandomValue();
                break;
            case NightMapStateEnum.MONSTER:
                ((NightMonster)nmo).data = m_NightMonsterChanceSheet.RandomValue();
                break;
        }
        return instance;
    }
    private void InstantiatePlayer(Vector2Int pos) {
        Player = InstantiateTile(m_playerPrefab, pos.x, pos.y, parent: m_playerParent).GetComponent<NightPlayerObject>();
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
            if (!CanGoAsShadow(index)) {
                return;
            }
        }
        if (index == Player.Position) {
            return;
        }
        m_paths = GoTo(index);
    }
    private void Move() {
        Vector3 target = new Vector3(m_paths[0].y, -m_paths[0].x, Player.transform.position.z);
        if (Vector3.Distance(Player.transform.position, target) <= 0.1f) {
            Player.transform.localPosition = target;
            Player.Position = m_paths[0];
            ShowMap(Player.Position);
            m_paths.RemoveAt(0);
        } else {
            Player.transform.Translate( m_playerSpeed * Time.deltaTime * (target - Player.transform.position));
        }
    }
    private List<Vector2Int> GoTo(Vector2Int target) {
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
        int TIMES = 0;
        while (path.Count > 0) {
            List<Vector2Int> currList = path.Dequeue();
            if ( currList.Count >= 2 * N) {
                // Debug.Log(string.Format("CAN NOT GO {0}", target))
                continue;
            }
            if (TIMES++ >= N * N) {
                // Debug.Log(string.Format("CAN NOT GO {0}", target))
                return new List<Vector2Int>();
            }
            Vector2Int current = currList[^1];
            if (current == target) {
                return currList;    
            } else {
                if (m_shadow[current.x, current.y]) {
                    continue;
                }
            }
            MarkVisited(current);
            Vector2Int left = new Vector2Int(current.x - 1, current.y);
            if (CanGo(left) && CheckVisited(left)) {
                List<Vector2Int> toAdd = new(currList);
                toAdd.Add(new Vector2Int(left.x, left.y));
                path.Enqueue(toAdd);
            }
            Vector2Int right = new Vector2Int(current.x + 1, current.y);
            if (CanGo(right) && CheckVisited(right)) {
                List<Vector2Int> toAdd = new(currList);
                toAdd.Add(new Vector2Int(right.x, right.y));
                path.Enqueue(toAdd);
            }
            Vector2Int up = new Vector2Int(current.x, current.y - 1);
            if (CanGo(up) && CheckVisited(up)) {
                List<Vector2Int> toAdd = new(currList);
                toAdd.Add(new Vector2Int(up.x, up.y));
                path.Enqueue(toAdd);
            }
            Vector2Int down = new Vector2Int(current.x, current.y + 1);
            if (CanGo(down) && CheckVisited(down)) {
                List<Vector2Int> toAdd = new(currList);
                toAdd.Add(new Vector2Int(down.x, down.y));
                path.Enqueue(toAdd);
            }
        }
        return new List<Vector2Int>();
    }
    bool CanGo(Vector2Int pos) {
        int x = pos.x;
        int y = pos.y;
        return InMap(x, y) && CanGoAsShadow(pos);   
    }
    private int HandlePointToArrayIndex(float p) {
        return Mathf.RoundToInt(p);
    }
    public bool InMap(int x, int y) {
        return x >= 0 && y >= 0  && x < m_nightMapChanceSheet.MapCount && y < m_nightMapChanceSheet.MapCount;
    }
    private bool CheckShadow(int x, int y) {
        return InMap(x, y) && !m_shadow[x, y];
    }
    private bool CanGoAsShadow(Vector2Int index) {
        if (CheckShadow(index.x, index.y)) {
            return true;
        }
        return CheckShadow(index.x - 1, index.y) || CheckShadow(index.x + 1, index.y) ||
               CheckShadow(index.x, index.y - 1) || CheckShadow(index.x, index.y + 1);
    }
    public void ShowMap(Vector2Int pos) {
        ShowShadow(pos.x, pos.y);
        NightMapStateEnum target = m_map[pos.x, pos.y];
        switch (target) {
            case NightMapStateEnum.CHEST: 
                Debug.Log("OPEN CHEST"); 
                break;
            case NightMapStateEnum.PROP: 
                Debug.Log("拾取物品");
                m_map[pos.x, pos.y] = NightMapStateEnum.SHOWED;
                GetTargetChild(m_mapParent, pos).SetActive(false);
                MainScene.PlayerState.AddToBackpack(GetTargetChildScript<NightProp>(m_mapParent, pos).data.items);
                break;
            case NightMapStateEnum.PATH: 
                // Debug.Log("GO TO THE PATH"); 
                m_map[pos.x, pos.y] = NightMapStateEnum.SHOWED;
                GetTargetChild(m_mapParent, pos).SetActive(false);
                break;
            case NightMapStateEnum.MONSTER:
                Battle(pos);
                break;
        }
        /*if (InMap(pos.x - 1, pos.y)) {
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
        }*/
    }
    private void ShowShadow(int r, int c) {
        m_shadow[r, c] = false;
        Transform shadow = m_shadowParent.GetChild(r * m_nightMapChanceSheet.MapCount + c);
        shadow.gameObject.SetActive(false);
    }
    private void Battle(Vector2Int pos) {
        Debug.Log("BATTLE, 假设战胜了！！！");
        NightMonster monster = GetTargetChildScript<NightMonster>(m_mapParent, pos);
        NightMonsterData data = monster.data;
        MainScene.PlayerState.DreamEnergy -= data.Damage;
        MainScene.PlayerState.NightmareCrystalline += data.NightmareCrystalline;
        MainScene.PlayerState.AddToBackpack(data.Dropping);
        monster.gameObject.SetActive(false);
        m_map[pos.x, pos.y] = NightMapStateEnum.SHOWED;
    }

    private GameObject GetTargetChild(Transform parent, Vector2Int pos) {
        return parent.GetChild(pos.x * m_nightMapChanceSheet.MapCount + pos.y).gameObject;;
    }
    private T GetTargetChildScript<T>(Transform parent, Vector2Int pos) {
        return GetTargetChild(parent, pos).GetComponent<T>();
    }
}
