using UnityEngine;
public class NightMapObject : MonoBehaviour {
    /// <summary>
    /// ��[r, c]����ʽ����
    /// </summary>
    public Vector2 Position { get; set; }
    [SerializeField] private NightMapStateEnum m_state;
    public NightMapStateEnum State => m_state;
}
