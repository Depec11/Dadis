using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Frame.UI {
    public enum DragType { 
        Self = 0x0,
        Parent = 0x1,
    }
    /// <summary>
    /// UI拖拽控制器，继承自单一行为，拥有接口拖拽处理
    /// </summary>
    public class UIWindowed : MonoBehaviour, IDragHandler {
        [Tooltip("窗口的矩形变化"), SerializeField] private RectTransform rectTransform;
        [Tooltip("关闭按钮"), SerializeField] protected Button m_closeButton;
        [Tooltip("拖拽移动类型"), SerializeField] private DragType m_dragType = DragType.Self;
        [Tooltip("关闭按钮按下后运行的事件")] public event vn OnClose;
        protected virtual void Awake() {
            m_closeButton.onClick.AddListener(OnCloseButtonDown);
        }
        /// <summary>
        /// 拖拽时运行的函数
        /// </summary>
        /// <param name="eventData">拖拽事件</param>
        public virtual void OnDrag(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                switch (m_dragType) {
                    case DragType.Self: rectTransform.transform.position += new Vector3(eventData.delta.x, eventData.delta.y); break;
                    case DragType.Parent: rectTransform.parent.transform.position += new Vector3(eventData.delta.x, eventData.delta.y); break;
                    default: break;
                }
            }
        }
        /// <summary>
        /// 关闭按钮按下后运的行函数
        /// </summary>
        protected virtual void OnCloseButtonDown() { 
            OnClose?.Invoke();
            OnClose = null;
            Destroy(gameObject);
        }
    }
}
