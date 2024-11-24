using TMPro;
using Frame.UI;
using UnityEngine;
namespace Frame.Event { 
    /// <summary>
    /// 事件行为
    /// </summary>
    public sealed class EventBehaviour : UIWindowed {
        [Tooltip("标题UI"), SerializeField] private TextMeshProUGUI m_titleUI;
        [Tooltip("文本UI"), SerializeField] private TextMeshProUGUI m_contentUI;
        /*[Tooltip("刷新文本"), SerializeField] private Button m_flush;*/
        [Tooltip("事件消息")] private EventMessage m_msg;
        protected override void Awake() {
            base.Awake();
            /*m_flush.onClick.AddListener(Flush);*/
            // GetComponent<LocalizeStringEvent>().OnUpdateString.AddListener((v) => { Flush(); });
        }
        /// <summary>
        /// 刷新文本
        /// </summary>
        private void Flush() {
            try {
                m_titleUI.text = m_msg.getTitle();
                m_contentUI.text = m_msg.getContent();
            } catch { }
        }
        public void Init(EventMessage msg) {
            m_msg = msg;
            Flush();
            OnClose += msg.onEnd;
        }
    }
}
