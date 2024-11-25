using System;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/NightMapConfig"), fileName = ("NightMapConfig_"))]
[Serializable] public class NightMapConfig : ScriptableObject {
    public NightMonsterData monsterDataA;
    public NightMonsterData monsterDataB;
    public NightChestData chestDataA;
    public NightChestData chestDataB;
    public GameObject trapPrefab;
}
