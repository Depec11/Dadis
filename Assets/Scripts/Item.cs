using System;
using UnityEngine;

/// <summary>
/// 物品，继承自物体
/// </summary>
public abstract class Item { 
    /// <summary>
    /// 数量
    /// </summary>
    public int count = 1;
    /// <summary>
    /// 等级
    /// </summary>
    public int level = 1;
    /// <summary>
    /// 类型
    /// </summary>
    public Flag.ItemType type = Flag.ItemType.Default;
    /// <summary>
    /// 是否可以被消耗耐久
    /// </summary>
    public bool unbrokenable = false;
    /// <summary>
    /// 耐久，-1为正无穷
    /// </summary>
    public int duration = -1;
    /// <summary>
    /// 最大堆叠数，-1为正无穷
    /// </summary>
    private int m_maxCount;
    /// <summary>
    /// 名称
    /// </summary>
    protected string m_name = "DefaultItem";
    /// <summary>
    /// 名称
    /// </summary>
    public string Name => m_name;
    /// <summary>
    /// 最大堆叠数
    /// </summary>
    public int MaxCount => m_maxCount;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="l">等级</param>
    /// <param name="c">数量</param>
    /// <param name="d">耐久</param>
    /// <param name="mc">最大堆叠数</param>
    /// <param name="t">类型</param>
    /// <param name="n">名称</param>
    public Item(int l, int c, int d, int mc, Flag.ItemType t = Flag.ItemType.Default, string n = "DefaultItem") {
        level = l;
        count = c;
        duration = d;
        m_maxCount = mc;
        type = t;
        m_name = n;
        if (duration == -1) unbrokenable = true;
    }
    /// <summary>
    /// 重载等于判断
    /// </summary>
    /// <param name="i1">物品1</param>
    /// <param name="i2">物品2</param>
    /// <returns>是否相等</returns>
    public static bool operator ==(Item i1, Item i2) {
        if (ReferenceEquals(i1, null) && ReferenceEquals(i2, null)) return true;
        if (ReferenceEquals(i1, null) || ReferenceEquals(i2, null)) return false;
        return i1.Name == i2.Name && i1.level == i2.level && i1.duration == i2.duration;
    }
    /// <summary>
    /// 重载不等于判断
    /// </summary>
    /// <param name="i1">物品1</param>
    /// <param name="i2">物品2</param>
    /// <returns>是否不相等</returns>
    public static bool operator !=(Item i1, Item i2) {
        return !(i1 == i2);
    }
    /// <summary>
    /// 重载判断是否相等
    /// </summary>
    /// <param name="obj">比较对象</param>
    /// <returns>是否相等</returns>
    public override bool Equals(object obj) {
        return this == (Item)obj;
    }
    /// <summary>
    /// 重载获取哈希值
    /// </summary>
    /// <returns>哈希值</returns>
    public override int GetHashCode() {
        return base.GetHashCode();
    }
    /// <summary>
    /// 重载转为字符串
    /// </summary>
    /// <returns>转化后的字符串</returns>
    public override string ToString() {
        return m_name;
    }
    /// <summary>
    /// 设置图片
    /// </summary>
    /// <param name="picture">图片渲染器的图片容器</param>
    public abstract Sprite Picture();
    /// <summary>
    /// 获取描述
    /// </summary>
    /// <returns>描述</returns>
    public virtual string GetDesc() {
        return "";
    }
    /// <summary>
    /// 判断是否能在背包UI中显示交互按钮
    /// </summary>
    /// <returns>能否显示</returns>
    public virtual bool CanShowInteractiveButton() {
        return false;
    }
#if DEBUG
    /// <summary>
    /// 复制物品
    /// </summary>
    /// <param name="item">物品</param>
    /// <returns>复制后的物品</returns>
    public static Item Copy(Item item) { 
        Type type = item.GetType();
        return (Item)Activator.CreateInstance(type);
    }
#endif
    /// <summary>
    /// 获取可交互文本
    /// </summary>
    /// <returns>可交互文本</returns>
    public virtual string GetInteractiveText() {
        return "";
    }
    /// <summary>
    /// 销毁物品
    /// </summary>
    public virtual void Dispose() { }
}