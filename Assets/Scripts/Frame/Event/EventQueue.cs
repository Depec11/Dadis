using System.Collections.Generic;
using UnityEngine; 
using Frame.UI;
namespace Frame.Event { 
    /// <summary>
    /// 事件信息
    /// </summary>
    public readonly struct EventMessage {
        [Tooltip("事件标题")] public readonly sn getTitle;
        [Tooltip("事件文本")] public readonly sn getContent;
        [Tooltip("事件后运行的函数")] public readonly vn onEnd;
        [Tooltip("事件唤醒后运行的函数")] public readonly vn onDequeue;
        [Tooltip("延迟响应事件")] public readonly float delay;
        [Tooltip("是否显示UI")] public readonly bool isShowUI;
        /// <param name="title">标题</param>
        /// <param name="content">文本</param>
        /// <param name="onEnd">事件结束后运行的函数</param>
        /// <param name="onDequeue">事件离开事件队列后运行的函数</param>
        /// <param name="delay">延迟时间</param>
        /// <param name="isShowUI">是否使用事件UI</param>
        public EventMessage(string title, string content, vn onEnd, vn onDequeue, float delay, bool isShowUI) { 
            this.getTitle = () => { return title; };
            this.getContent = () => { return content; };
            this.onEnd = onEnd;
            this.onDequeue = onDequeue;
            this.delay = delay;
            this.isShowUI = isShowUI;
        }
        /// <param name="title">标题</param>
        /// <param name="content">文本</param>
        /// <param name="onEnd">事件结束后运行的函数</param>
        /// <param name="onDequeue">事件离开事件队列后运行的函数</param>
        /// <param name="delay">延迟时间</param>
        /// <param name="isShowUI">是否使用事件UI</param>
        public EventMessage(sn title, sn content, vn onEnd, vn onDequeue, float delay, bool isShowUI) {
            this.getTitle = title;
            this.getContent = content;
            this.onEnd = onEnd;
            this.onDequeue = onDequeue;
            this.delay = delay;
            this.isShowUI = isShowUI;
        }
    }
    /// <summary>
    /// 事件队列
    /// </summary>
    public sealed class EventQueue {
        [Tooltip("事件窗口悬挂的UI")] private readonly Transform m_uiParent;
        [Tooltip("事件消息队列")] private readonly Queue<EventMessage> m_messageQueue = new();
        [Tooltip("事件UI控制器")] private readonly UIController m_eventUI;
        [Tooltip("积累的时长")] private float m_delay = 0;
        public EventQueue(GameObject eventUI, Transform uiParent) { 
            m_uiParent = uiParent;
            m_eventUI = new UIController(eventUI, uiParent);
        }
        public void Update() {
            if (!m_eventUI.Exists && m_messageQueue.Count > 0) {
                EventMessage eventMsg = m_messageQueue.Peek();
                if (eventMsg.delay <= m_delay) {
                    m_delay = 0;
                    eventMsg.onDequeue?.Invoke();
                    if (eventMsg.isShowUI) {
                        m_eventUI.Generate();
                        m_eventUI.GetComponent<EventBehaviour>().Init(m_messageQueue.Dequeue());
                    } else { m_messageQueue.Dequeue(); }
                }
                else { m_delay += Time.deltaTime; }
            }
        }
        /// <param name="title">标题</param>
        /// <param name="content">文本</param>
        /// <param name="onEnd">事件结束后运行的函数</param>
        /// <param name="onDequeue">事件离开事件队列后运行的函数</param>
        /// <param name="delay">延迟时间</param>
        /// <param name="isShowUI">是否使用事件UI</param>
        public void PushEvent(string title, string content, vn onEnd = null, vn onDequeue = null, float delay = 0, bool isShowUI = true) {
            m_messageQueue.Enqueue(new EventMessage(title, content, onEnd, onDequeue, delay, isShowUI));
        }
        /// <param name="title">标题</param>
        /// <param name="content">文本</param>
        /// <param name="onEnd">事件结束后运行的函数</param>
        /// <param name="onDequeue">事件离开事件队列后运行的函数</param>
        /// <param name="delay">延迟时间</param>
        /// <param name="isShowUI">是否使用事件UI</param>
        public void PushEvent(sn title, sn content, vn onEnd = null, vn onDequeue = null, float delay = 0, bool isShowUI = true) {
            m_messageQueue.Enqueue(new EventMessage(title, content, onEnd, onDequeue, delay, isShowUI));
        }
        /// <param name="onEnd">事件结束后运行的函数</param>
        /// <param name="onDequeue">事件离开事件队列后运行的函数</param>
        /// <param name="delay">延迟时间</param>
        public void PushEvent(vn onEnd = null, vn onDequeue = null, float delay = 0) {
            m_messageQueue.Enqueue(new EventMessage("", "", onEnd, onDequeue, delay, false));
        }
    }
}
