using System;
using Frame.Data;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = ("Data/MapChanceSheet"), fileName = ("MapChanceSheet_"))]
[Serializable] public class NightMapSettings : ChanceSheet<NightMapStateEnum> {
    [Tooltip("地图的大小 即为N*N中的N")][Range(5, 20)][SerializeField] private int m_MapSize;
    public NightMapConfig mapConfigure;
    public int MapSize => m_MapSize;
}
