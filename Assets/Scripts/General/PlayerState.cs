using UnityEngine;
using System.Collections.Generic;
public class PlayerState {
    /// <summary>
    /// 梦境能量
    /// </summary>
    public int DreamEnergy { get; set; }
    /// <summary>
    /// 噩梦抗性
    /// </summary>
    public int NightHitPoint { get; set; }
    /// <summary>
    /// 情绪能量
    /// </summary>
    public int EmotionEnergy { get; set; }
    /// <summary>
    /// 背包
    /// </summary>
    public Backpack Backpack { get; set; }
    public const int MAX_NIGHT_HIT_POINTS = 100;
    public PlayerState() {
        DreamEnergy = 0;
        NightHitPoint = MAX_NIGHT_HIT_POINTS;
        EmotionEnergy = 0;
        Backpack = new Backpack();
    }
    /// <summary>
    /// 添加到背包
    /// </summary>
    public void AddToBackpack(Item item) {
        Backpack.Add(item);
    }
    
    public void AddToBackpack(List<Item> items) {
        foreach (var it in items) {
            Backpack.Add(it);
        }
    }
    /// <summary>
    /// 移除背包
    /// </summary>
    public void RemoveFromBackpack(Item item) {
        Backpack.Remove(item);
    }
}
