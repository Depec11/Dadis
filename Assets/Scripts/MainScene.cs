// ENCODING WITH UTF-8

using UnityEngine;
using static UsefulFunctions;

public class MainScene : MonoBehaviour {
    /// <summary>
    /// 白天场景
    /// </summary>
    [SerializeField] private GameObject m_dayScenes;
    /// <summary>
    /// 夜晚场景
    /// </summary>
    [SerializeField] private GameObject m_nightScenes;
    private void Awake() {
        GenerateNightScene();
    }
    /// <summary>
    /// 同步生成白天场景，会摧毁其他场景
    /// </summary>
    public void GenerateDayScene() {
        DestroyAllChildren(transform);
        Instantiate(m_dayScenes, transform);
    }
    /// <summary>
    /// 同步生成夜晚场景，会摧毁其他场景
    /// </summary>
    public void GenerateNightScene() {
        DestroyAllChildren(transform);
        Instantiate (m_nightScenes, transform);
    }
}
