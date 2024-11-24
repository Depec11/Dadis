using FunctionTemplates;
using UnityEngine;

namespace UI {
    /// <summary>
    /// UI控制器
    /// </summary>
    public sealed class UIController {
        [Tooltip("预制体")] private readonly GameObject m_prefab;
        [Tooltip("管理的UI")] private Transform m_ui;
        [Tooltip("UI的父级")] private Transform m_parent;
        /// <summary>
        /// 返回生成的UI
        /// </summary>
        public GameObject UI => m_ui.gameObject;
        /// <summary>
        /// 返回UI物体是否被创建
        /// </summary>
        public bool Exists => m_ui != null;
        /// <summary>
        /// 返回父级
        /// </summary>
        public Transform Parent => m_parent;
        /// <summary>
        /// 初始化UI控制器
        /// </summary>
        /// <param name="parent">父级</param>
        /// <param name="instantGenerate">是否立刻生成</param>
        /// <param name="onGeneratingEnd">UI生成后运行的函数</param>
        public UIController(GameObject prefab, Transform parent = null, bool instantGenerate = false, vt1 onGeneratingEnd = null) {
            m_prefab = prefab;
            m_parent = parent;
            if (instantGenerate) { Generate(onGeneratingEnd); }
        }
        /// <summary>
        /// 生成UI
        /// </summary>
        /// <param name="onGeneratingEnd">UI生成后运行的函数</param>
        public void Generate(vt1 onGeneratingEnd = null) {
            if (Exists) { Destroy(); }
            m_ui = Object.Instantiate(m_prefab, m_parent).transform;
            onGeneratingEnd?.Invoke(m_ui);
        }
        /// <summary>
        /// 删除UI
        /// </summary>
        public void Destroy(vn t = null) {
            if (Exists) {
                t?.Invoke();
                Object.Destroy(m_ui.gameObject);
                m_ui = null;
            }
        }
        /// <summary>
        /// 获取UI的组件
        /// </summary>
        /// <typeparam name="T">组件类</typeparam>
        /// <returns>组件</returns>
        public T GetComponent<T>() where T : Component {
            if (m_ui == null) { return default; }
            return m_ui.GetComponent<T>();
        }
        /// <summary>
        /// 设置父类
        /// </summary>
        /// <param name="parent">父类</param>
        public void SetParent(Transform parent) {
            m_parent = parent;
            if (Exists) { m_ui.parent = parent; }
        }
    }
}
