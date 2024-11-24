using System.Collections.Generic;
using UnityEngine;
public class NightManager : SceneManager {
    [SerializeField] private MapChanceSheet m_mapChanceSheet;
    private NightMapStateEnum[,] m_map;
    protected override void Awake() {
        base.Awake();
        GenerateMap();
    }
    private void GenerateMap() {
        m_mapChanceSheet.Check();
        m_map = new NightMapStateEnum[m_mapChanceSheet.MapCount, m_mapChanceSheet.MapCount];
        // List<NightMapStateEnum> temp = new();
        NightMapStateEnum[] sample = new NightMapStateEnum[4] { NightMapStateEnum.CHEST, NightMapStateEnum.PROP, NightMapStateEnum.PATH, NightMapStateEnum.MONSTER };
        for (int i = 0; i < m_mapChanceSheet.MapCount; i++) {
            for (int j = 0; j < m_mapChanceSheet.MapCount; j++) {
                m_map[i, j] = m_mapChanceSheet.GetRandomValue(sample);
            }
        }
    }
}
