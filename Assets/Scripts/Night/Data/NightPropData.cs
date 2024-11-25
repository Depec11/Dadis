using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("Data/PropData"), fileName = ("PropData_"))]
[Serializable] public class NightPropData : ScriptableObject {
    public List<Item> items = new List<Item>();
}
