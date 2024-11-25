using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class NightManager : Frame.SceneManager {
    public static NightManager Instance { get; private set; }
    [SerializeField] private NightMapSettings m_nightMapChanceSettings;
    /// <summary>
    ///  [i, j]，i是行，j是列
    /// </summary>
    private NightMapStateEnum[,] m_map;
    private bool[,] m_shadow;
    [SerializeField] private Transform m_mapParent;
    [SerializeField] private Transform m_shadowParent;
    [SerializeField] private Transform m_playerParent;
    [SerializeField] private GameObject m_chestPrefab;
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
        m_map = new NightMapStateEnum[m_nightMapChanceSettings.MapSize, m_nightMapChanceSettings.MapSize];
        m_shadow = new bool[m_nightMapChanceSettings.MapSize, m_nightMapChanceSettings.MapSize];
        List<Vector2Int> paths = new();
        for (int i = 0; i < m_nightMapChanceSettings.MapSize; i++) {
            for (int j = 0; j < m_nightMapChanceSettings.MapSize; j++) {
                m_shadow[i, j] = true;
                InstantiateTile(m_shadowPrefab , i,j, parent:m_shadowParent);
                m_map[i, j] = m_nightMapChanceSettings.RandomValue();
                switch (m_map[i, j]) {
                    case NightMapStateEnum.CHEST:
                        InstantiateTile(m_chestPrefab, i, j, NightMapStateEnum.CHEST, m_mapParent);
                        break;
                    case NightMapStateEnum.PATH:
                        paths.Add(new Vector2Int(i, j));
                        InstantiateTile(m_pathPrefab, i, j, parent:m_mapParent);
                        break;
                    case NightMapStateEnum.TRAP:
                        InstantiateTile(m_nightMapChanceSettings.mapConfigure.trapPrefab, i, j, NightMapStateEnum.TRAP, m_mapParent);
                        break;
                    case NightMapStateEnum.MONSTER_A:
                        InstantiateTile(m_monsterPrefab, i, j, NightMapStateEnum.MONSTER_A, m_mapParent);
                        break; 
                    case NightMapStateEnum.MONSTER_B:
                        InstantiateTile(m_monsterPrefab, i, j, NightMapStateEnum.MONSTER_B, m_mapParent);
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
                ((NightChest)nmo).data = Random.Range(0, 2) == 0 ? m_nightMapChanceSettings.mapConfigure.chestDataA : m_nightMapChanceSettings.mapConfigure.chestDataB;
                break;
            case NightMapStateEnum.MONSTER_A:
                ((NightMonster)nmo).data = m_nightMapChanceSettings.mapConfigure.monsterDataA;
                break;
            case NightMapStateEnum.MONSTER_B:
                ((NightMonster)nmo).data = m_nightMapChanceSettings.mapConfigure.monsterDataB;
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
        int N = m_nightMapChanceSettings.MapSize;
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
    public void ShowMap(Vector2Int pos) {
        ShowShadow(pos.x, pos.y);
        NightMapStateEnum target = m_map[pos.x, pos.y];
        switch (target) {
            case NightMapStateEnum.CHEST: 
                m_map[pos.x, pos.y] = NightMapStateEnum.SHOWED;
                GetTargetChild(m_mapParent, pos).SetActive(false);
                MainScene.PlayerState.AddToBackpack(GetTargetChildScript<NightChest>(m_mapParent, pos).data.items);
                break;
            case NightMapStateEnum.PATH: 
                m_map[pos.x, pos.y] = NightMapStateEnum.SHOWED;
                GetTargetChild(m_mapParent, pos).SetActive(false);
                break;
            case NightMapStateEnum.MONSTER_A:
                Battle(pos);
                break;
            case NightMapStateEnum.MONSTER_B:
                Battle(pos);
                break;
            case NightMapStateEnum.TRAP:
                GetTargetChildScript<NightTrap>(m_mapParent, pos).OnPlayerOver();
                break;
        }
    }
    private void ShowShadow(int r, int c) {
        m_shadow[r, c] = false;
        Transform shadow = m_shadowParent.GetChild(r * m_nightMapChanceSettings.MapSize + c);
        shadow.gameObject.SetActive(false);
    }
    private void Battle(Vector2Int pos) {
        NightMonster monster = GetTargetChildScript<NightMonster>(m_mapParent, pos);
        NightMonsterData data = monster.data;
        MainScene.PlayerState.NightHitPoint -= data.Damage;
        if (MainScene.PlayerState.NightHitPoint <= 0) {
            Debug.Log("YOU DIE");
        }
        MainScene.PlayerState.AddToBackpack(data.Dropping);
        monster.gameObject.SetActive(false);
        m_map[pos.x, pos.y] = NightMapStateEnum.SHOWED;
    }
    /*从此开始是是基础架构*/
    private GameObject GetTargetChild(Transform parent, Vector2Int pos) {
        return parent.GetChild(pos.x * m_nightMapChanceSettings.MapSize + pos.y).gameObject;;
    }
    private T GetTargetChildScript<T>(Transform parent, Vector2Int pos) {
        return GetTargetChild(parent, pos).GetComponent<T>();
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
        return x >= 0 && y >= 0  && x < m_nightMapChanceSettings.MapSize && y < m_nightMapChanceSettings.MapSize;
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
}
