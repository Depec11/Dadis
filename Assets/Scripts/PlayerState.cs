using UnityEngine;

public class PlayerState {
    /// <summary>
    /// 梦境能量
    /// </summary>
    public int DreamEnergy { get; set; }
    /// <summary>
    /// 噩梦抗性
    /// </summary>
    public int NightDefense { get; set; }
    /// <summary>
    /// 情绪能量
    /// </summary>
    public int EmotionEnergy { get; set; }
    /// <summary>
    /// 噩梦结晶
    /// </summary>
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
