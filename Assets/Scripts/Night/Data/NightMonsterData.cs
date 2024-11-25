using System;
using Flag;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("Data/MonsterData"), fileName = ("MonsterData_"))]
[Serializable]  public class NightMonsterData : ScriptableObject {
    [SerializeField] private int m_damage;
    [SerializeField] private int m_dreamCore;
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private List<Item> m_dropping;
    public int Damage => m_damage;
    public int DreamCore => m_dreamCore;
    public GameObject Prefab => m_prefab;
    public List<Item> Dropping => m_dropping;
}
