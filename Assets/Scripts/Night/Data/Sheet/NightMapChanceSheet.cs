using System;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/MapChanceSheet"), fileName = ("MapChanceSheet_"))]
[Serializable] public class NightMapChanceSheet : ScriptableObject {
    [Tooltip("地图的大小 即为N*N中的N")][Range(5, 20)][SerializeField] private int m_mapCount;
    [Tooltip("箱子的概率")][Range(0.1f, 0.9f)][SerializeField] private float m_chestChance;
    [Tooltip("道具的概率")][Range(0.1f, 0.9f)][SerializeField] private float m_propChance;
    [Tooltip("路径的概率")][Range(0.1f, 0.9f)][SerializeField] private float m_pathChance;
    [Tooltip("怪兽的概率")][Range(0.1f, 0.9f)][SerializeField] private float m_monsterChance;
    public int MapCount => m_mapCount;
    public float ChestChance => m_chestChance;
    public float PropChance => m_propChance;
    public float PathChance => m_pathChance;
    public float MonsterChance => m_monsterChance;
    public void Check() {
        float sum = m_chestChance + m_propChance + m_pathChance + m_monsterChance;
        m_chestChance /= sum;
        m_propChance /= sum;
        m_pathChance /= sum;
        m_monsterChance /= sum;
    }
    public T GetRandomValue<T>(T[] t_array) {
        float r = UnityEngine.Random.Range(0.0f, 1.0f);
        r -= m_chestChance;
        if (r < 0) { 
            return t_array[0];
        }
        r -= m_propChance;
        if(r < 0) {
            return t_array[1];
        }
        r -= m_pathChance;
        if(r < 0) {
            return t_array[2];
        }
        r -= m_monsterChance;
        if (r < 0) {
            return t_array[3];
        }
        return t_array[0];
    }
}
