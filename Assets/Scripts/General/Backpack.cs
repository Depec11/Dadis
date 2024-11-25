using System.Collections.Generic;
using static Frame.UsefulFunctions;
/// <summary>
/// 背包
/// </summary>
public class Backpack {
    /// <summary>
    /// 物品表
    /// </summary>
    public List<Item> items = new();
    public Backpack() { }
    /// <summary>
    /// 添加物品
    /// </summary>
    /// <param name="t">物品</param>
    public void Add(Item t) {
        for (int i = 0; i < items.Count; i++) {
            if (t == items[i]) {
                if (items[i].maxCount == -1) {
                    items[i].count += t.count;
                    return;
                } else if (items[i].maxCount == items.Count) {
                    break;
                }
            }
        }
        items.Add(t);
    }
    /// <summary>
    /// 重载变为字符串
    /// </summary>
    /// <returns>转为字符串的结果</returns>
    public override string ToString() {
        string result = "";
        for (int i = 0; i < items.Count; i++) {
            result += items[i].ToString() + " -> " + items[i].count.ToString() + "\n";
        }
        return result;
    }
    /// <summary>
    /// 判断是否包含某种物体
    /// </summary>
    /// <param name="itemFlags">物体类型</param>
    /// <returns>是否包含</returns>
    public bool ContainsItemsWithFlags(params Flag.ItemType[] itemFlags) {
        if (itemFlags.Length == 0) return true;
        for (int i = 0; i < items.Count; i++) {
            if (ArrayContains(itemFlags, items[i].type)) {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 移除物体
    /// </summary>
    /// <param name="it">物体</param>
    public void Remove(Item it) { 
        items.Remove(it);
        it.Dispose();
    }
    /// <summary>
    /// 清空
    /// </summary>
#if DEBUG
    public void Clear() {
        foreach (Item it in items) {
            it.Dispose();
        }
        items.Clear();
    }
#endif
    /// <summary>
    /// 删除无耐久和无数量的
    /// </summary>
    public void DisposeItems() { 
        for (int i = 0; i < items.Count; i++) {
            Item it = items[i];
            if (it.count <= 0 || (!it.unbrokenable && it.duration <= 0)) {
                Remove(it);
            }
        }
    }
    /// <summary>
    /// 是否含有某种元素
    /// </summary>
    /// <param name="name">名字</param>
    /// <param name="level">等级</param>
    /// <param name="count">数量</param>
    /// <returns>是否具有</returns>
    public bool Seacrch(int uid, int level, int count) {
        foreach (Item it in items) {
            if (it.uid == uid && it.level == level && it.count >= count) {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 获得特定的物品
    /// </summary>
    /// <param name="uid">UID</param>
    /// <param name="level">等级</param>
    /// <returns>物品</returns>
    public List<Item> Get(int uid, int level) {
        List<Item> result = new();
        foreach (Item it in items) {
            if (it.uid == uid && it.level == level) {
                result.Add(it);
            }
        }
        return result;
    }
}
