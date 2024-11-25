using System;
using Flag;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("Data/MonsterData"), fileName = ("MonsterData_"))]
[Serializable]  public class NightMonsterData : ScriptableObject {
    [SerializeField] private string m_name;
    [SerializeField] private int m_damage;
    [SerializeField] private int m_nightmareCrystalline;
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private List<Item> m_dropping;
    public string NAme => m_name;
    public int Damage => m_damage;
    public int NightmareCrystalline => m_nightmareCrystalline;
    public GameObject Prefab => m_prefab;
    public List<Item> Dropping => m_dropping;
}
