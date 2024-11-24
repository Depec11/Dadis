using System;
using System.Collections.Generic;

/// <summary>
/// 序偶
/// </summary>
/// <typeparam name="T1">第一个元素类型</typeparam>
/// <typeparam name="T2">第一个元素类型</typeparam>
[Serializable]
public class Pair<T1, T2> {
    /// <summary>
    /// 第一个元素
    /// </summary>
    public T1 first;
    /// <summary>
    /// 第二个元素
    /// </summary>
    public T2 second;
    /// <summary>
    /// 初始化序偶
    /// </summary>
    /// <param name="f">第一个元素的值</param>
    /// <param name="c">第二个元素的值</param>
    public Pair(T1 f, T2 s) {
        first = f;
        second = s;
    }
    /// <summary>
    /// 重载等于判断
    /// </summary>
    /// <param name="a">序偶a</param>
    /// <param name="b">序偶b</param>
    /// <returns>是否相等</returns>
    public static bool operator ==(Pair<T1, T2> a, Pair<T1, T2> b) {
        return EqualityComparer<T1>.Default.Equals(a.first, b.first) && EqualityComparer<T2>.Default.Equals(a.second, b.second);
    }
    /// <summary>
    /// 重载不等于判断
    /// </summary>
    /// <param name="a">序偶a</param>
    /// <param name="b">序偶b</param>
    /// <returns>是否不相等</returns>
    public static bool operator !=(Pair<T1, T2> a, Pair<T1, T2> b) {
        return !(a == b);
    }
    /// <summary>
    /// 重载判断是否相等
    /// </summary>
    /// <param name="obj">比较对象</param>
    /// <returns>是否相等</returns>
    public override bool Equals(object obj) {
        if (!(obj is Pair<T1, T2>)) return false;
        Pair<T1, T2> other = (Pair<T1, T2>)obj;
        return EqualityComparer<T1>.Default.Equals(first, other.first) && EqualityComparer<T2>.Default.Equals(second, other.second);
    }
    /// <summary>
    /// 重载获取哈希值
    /// </summary>
    /// <returns>哈希值</returns>
    public override int GetHashCode() {
        return base.GetHashCode();
    }
    /// <summary>
    /// 重载变为字符串
    /// </summary>
    /// <returns>转为字符串的结果</returns>
    public override string ToString() {
        return string.Format("<{0}:{1}>", first, second);
    }
}