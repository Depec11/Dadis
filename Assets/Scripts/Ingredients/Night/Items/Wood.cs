using Flag;
using UnityEngine;

public class Wood : Item {
    public Wood(int l, int c, int d, int mc, ItemType t = ItemType.INGREDIENT, string n = "木头") : base(l, c, d, mc, t, n) { }
}