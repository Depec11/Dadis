using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 概率表，继承自可编写对象
/// </summary>
/// <typeparam name="T">概率对应的返回类型</typeparam>
public class ChanceSheet<T> : ScriptableObject {
    /// <summary>
    /// 概率列表
    /// </summary>
    [SerializeField] List<Pair<float, T>> pairList;

    public ChanceSheet() { 
        if (pairList == null) pairList = new List<Pair<float, T>>();
    }
    /// <summary>
    /// 重载转字符串函数
    /// </summary>
    /// <returns>转换的字符串</returns>
#if DEBUG
    public override string ToString() {
        string text = "";
        for (int i = 0; i < pairList.Count; i++) {
            if (i != 0) text += "\n";
            text += pairList[i].ToString();
        }
        return text;
    }
#endif
    /// <summary>
    /// 添加序偶
    /// </summary>
    /// <param name="pair">序偶</param>
    public void Add(params Pair<float, T>[] pairs) {
        for (int i = 0; i < pairs.Length; i++) pairList.Add(pairs[i]);
    }
    /// <summary>
    /// 按概率寻找
    /// </summary>
    /// <param name="chance">概率</param>
    /// <returns>概率对应的对象</returns>
    public T SearchByChance(float chance) {
        for (int i = 0; i < pairList.Count; i++) {
            if (chance >= pairList[i].first) chance -= pairList[i].first;
            else return pairList[i].second;
        }
        return default;
    }
    /// <summary>
    /// 按值搜索
    /// </summary>
    /// <param name="value">值</param>
    /// <returns>值的序偶</returns>
    public Pair<float, T> SearchByValue(T value) {
        for (int i = 0; i < pairList.Count; i++) {
            if (EqualityComparer<T>.Default.Equals(pairList[i].second, value)) return pairList[i];
        }
        return new Pair<float, T>(0, value);
    }
    /// <summary>
    /// 获取随机的对象
    /// </summary>
    /// <returns>概率对应的对象</returns>
    public T RandomValue() { 
        return SearchByChance(Random.Range(0.0f, 1.0f));
    }

    public ChanceSheet<T> Copy() {
        Pair<float, T>[] tempList = new Pair<float, T>[pairList.Count];
        for (int i = 0; i < pairList.Count; i++) tempList[i] = new Pair<float, T>(pairList[i].first, pairList[i].second);
        ChanceSheet<T> t = new ChanceSheet<T>();
        t.Add(tempList);
        return t;
    }
}
