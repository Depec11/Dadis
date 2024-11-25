using System;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/MonsterData"), fileName = ("MonsterData_"))]
[Serializable] public class NightMonsterData : ScriptableObject {
    [SerializeField] private string m_name;
    [SerializeField] private int m_damage;
    [SerializeField] private int m_nightmareCrystalline;
    public string NAme => m_name;
    public int Damage => m_damage;
    public int NightmareCrystalline => m_nightmareCrystalline;
}
