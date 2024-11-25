using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = ("Data/ChestData"), fileName = ("ChestData_"))]
[Serializable] public class NightChestData : ScriptableObject {
    public List<Item> items;
}
