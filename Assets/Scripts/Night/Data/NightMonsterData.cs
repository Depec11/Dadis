using System;
using Flag;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("Data/MonsterData"), fileName = ("MonsterData_"))]
[Serializable]  public class NightMonsterData : ScriptableObject {
    [SerializeField] private AnimalType m_animalType;
    [SerializeField] private int m_damage;
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private List<Item> m_dropping;
    public AnimalType AnimalType => m_animalType;
    public int Damage => m_damage;
    public GameObject Prefab => m_prefab;
    public List<Item> Dropping => m_dropping;
}
