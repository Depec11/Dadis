using System;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/MonsterData"), fileName = ("MonsterData_"))]
[Serializable] public class NightMonsterData : ScriptableObject {
    [SerializeField] private string m_name;
    [SerializeField] private int m_damage;
    [SerializeField] private int m_nightmareCrystalline;
    [SerializeField] private GameObject m_prefab;
    public string NAme => m_name;
    public int Damage => m_damage;
    public int NightmareCrystalline => m_nightmareCrystalline;
    public GameObject Prefab => m_prefab;
}
