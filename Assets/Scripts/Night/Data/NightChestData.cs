using System;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = ("Data/ChestData"), fileName = ("ChestData_"))]
[Serializable] public class NightChestData : ScriptableObject {
    public List<Item> m_Items;
}
