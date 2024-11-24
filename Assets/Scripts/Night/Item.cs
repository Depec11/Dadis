using System;
using UnityEngine;

/// <summary>
/// ��Ʒ���̳�������
/// </summary>
public abstract class Item { 
    /// <summary>
    /// ����
    /// </summary>
    public int count = 1;
    /// <summary>
    /// �ȼ�
    /// </summary>
    public int level = 1;
    /// <summary>
    /// ����
    /// </summary>
    public Flag.ItemType type = Flag.ItemType.Default;
    /// <summary>
    /// �Ƿ���Ա������;�
    /// </summary>
    public bool unbrokenable = false;
    /// <summary>
    /// �;ã�-1Ϊ������
    /// </summary>
    public int duration = -1;
    /// <summary>
    /// ���ѵ�����-1Ϊ������
    /// </summary>
    private int m_maxCount;
    /// <summary>
    /// ����
    /// </summary>
    protected string m_name = "DefaultItem";
    /// <summary>
    /// ����
    /// </summary>
    public string Name => m_name;
    /// <summary>
    /// ���ѵ���
    /// </summary>
    public int MaxCount => m_maxCount;
    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="l">�ȼ�</param>
    /// <param name="c">����</param>
    /// <param name="d">�;�</param>
    /// <param name="mc">���ѵ���</param>
    /// <param name="t">����</param>
    /// <param name="n">����</param>
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
    /// ���ص����ж�
    /// </summary>
    /// <param name="i1">��Ʒ1</param>
    /// <param name="i2">��Ʒ2</param>
    /// <returns>�Ƿ����</returns>
    public static bool operator ==(Item i1, Item i2) {
        if (ReferenceEquals(i1, null) && ReferenceEquals(i2, null)) return true;
        if (ReferenceEquals(i1, null) || ReferenceEquals(i2, null)) return false;
        return i1.Name == i2.Name && i1.level == i2.level && i1.duration == i2.duration;
    }
    /// <summary>
    /// ���ز������ж�
    /// </summary>
    /// <param name="i1">��Ʒ1</param>
    /// <param name="i2">��Ʒ2</param>
    /// <returns>�Ƿ����</returns>
    public static bool operator !=(Item i1, Item i2) {
        return !(i1 == i2);
    }
    /// <summary>
    /// �����ж��Ƿ����
    /// </summary>
    /// <param name="obj">�Ƚ϶���</param>
    /// <returns>�Ƿ����</returns>
    public override bool Equals(object obj) {
        return this == (Item)obj;
    }
    /// <summary>
    /// ���ػ�ȡ��ϣֵ
    /// </summary>
    /// <returns>��ϣֵ</returns>
    public override int GetHashCode() {
        return base.GetHashCode();
    }
    /// <summary>
    /// ����תΪ�ַ���
    /// </summary>
    /// <returns>ת������ַ���</returns>
    public override string ToString() {
        return m_name;
    }
    /// <summary>
    /// ����ͼƬ
    /// </summary>
    /// <param name="picture">ͼƬ��Ⱦ����ͼƬ����</param>
    public abstract Sprite Picture();
    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <returns>����</returns>
    public virtual string GetDesc() {
        return "";
    }
    /// <summary>
    /// �ж��Ƿ����ڱ���UI����ʾ������ť
    /// </summary>
    /// <returns>�ܷ���ʾ</returns>
    public virtual bool CanShowInteractiveButton() {
        return false;
    }
#if DEBUG
    /// <summary>
    /// ������Ʒ
    /// </summary>
    /// <param name="item">��Ʒ</param>
    /// <returns>���ƺ����Ʒ</returns>
    public static Item Copy(Item item) { 
        Type type = item.GetType();
        return (Item)Activator.CreateInstance(type);
    }
#endif
    /// <summary>
    /// ��ȡ�ɽ����ı�
    /// </summary>
    /// <returns>�ɽ����ı�</returns>
    public virtual string GetInteractiveText() {
        return "";
    }
    /// <summary>
    /// ������Ʒ
    /// </summary>
    public virtual void Dispose() { }
}
