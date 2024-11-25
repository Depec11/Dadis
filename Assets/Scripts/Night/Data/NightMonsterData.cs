using System;
using Flag;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/MonsterData"), fileName = ("MonsterData_"))]
[Serializable]
public class NightMonsterData : ScriptableObject {
    [SerializeField] private string m_name;
    [SerializeField] private int m_damage;
    [SerializeField] private int m_nightmareCrystalline;
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private NightMonsterDropping m_droppingType;
    public string NAme => m_name;
    public int Damage => m_damage;
    public int NightmareCrystalline => m_nightmareCrystalline;
    public GameObject Prefab => m_prefab;
    public Item Dropping {
        get {
            switch (m_droppingType) {
                case NightMonsterDropping.SLIME_MUCOUS:
                    return new SlimeMucous(1, 1, -1, 999);
                case NightMonsterDropping.BAT_WINGS:
                    return new BatWings(1, 1, -1, 999);
                case NightMonsterDropping.DARK_FLOWER:
                    return new DarkFlower(1, 1, -1, 999);
                case NightMonsterDropping.WOOD:
                    return new Wood(1, 1, -1, 999);
                case NightMonsterDropping.WOOD_HEART:
                    return new WoodHeart(1, 1, -1, 999);
            }
            return null;
        }
    }
}
