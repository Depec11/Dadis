using UnityEngine;

namespace FunctionTemplates {
    /// <summary>
    /// 参数为bool，返回值为void的委托
    /// </summary>
    public delegate void vb1(bool b1);
    /// <summary>
    /// 无参数，返回值为void的委托
    /// </summary>
    public delegate void vn();
    /// <summary>
    /// 参数为GameObject，返回值为void的委托
    /// </summary>
    public delegate void vg1(GameObject go);
    /// <summary>
    /// 参数为Transform，返回值为void的委托
    /// </summary>
    public delegate void vt1(Transform t);
    /// <summary>
    /// 参数为T，返回值为void的委托
    /// </summary>
    public delegate void vT1<T>(T t);
    /// <summary>
    /// 无参数，返回值为string的委托
    /// </summary>
    public delegate string sn();
    /// <summary>
    /// 以一个数组为参数，返回值为void的委托
    /// </summary>
    public delegate void vA<T>(T[] t);
}
