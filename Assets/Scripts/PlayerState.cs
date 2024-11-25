using UnityEngine;

public class PlayerState {
    public int NightTimeEnergy { get; set; }
    public int NightDeffence { get; set; }
    public int EmotionEnergy { get; set; }
    public int NightmareCrystalline { get; set; }
    public Backpack Backpack { get; set; }
    /// <summary>
    /// 添加到背包
    /// </summary>
    public void AddToBackpack(Item item) {
        Backpack.Add(item);
    }
    /// <summary>
    /// 移除背包
    /// </summary>
    public void RemoveFromBackpack(Item item) {
        Backpack.Remove(item);
    }
}
