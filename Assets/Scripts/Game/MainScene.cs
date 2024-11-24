using UnityEngine;
using static UsefulFunctions;

public class MainScene : MonoBehaviour {
    [SerializeField] private GameObject m_dayScenes;
    [SerializeField] private GameObject m_nightScenes;
    private void Awake() {
        GenerateNightScene();
    }
    public void GenerateDayScene() {
        DestroyAllChildren(transform);
        Instantiate(m_dayScenes, transform);
    }
    public void GenerateNightScene() {
        DestroyAllChildren(transform);
        Instantiate (m_nightScenes, transform);
    }
}
