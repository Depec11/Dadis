using UnityEngine;
public class NightMapObject : MonoBehaviour {
    /// <summary>
    /// 以[r, c]的形式储存
    /// </summary>
    public Vector2Int Position { get; set; }
    [SerializeField] private NightMapStateEnum m_state;
    public NightMapStateEnum State => m_state;
}
