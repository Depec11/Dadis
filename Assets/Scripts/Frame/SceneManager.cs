using UnityEngine;
namespace Frame {
    public abstract class SceneManager : MonoBehaviour {
        /// <summary>
        /// UI的父级
        /// </summary>
        public static Transform UITransform { get; private set; }

        /// <summary>
        /// 自己
        /// </summary>
        public static Transform Transform { get; private set; }

        protected virtual void Awake() {
            UITransform = GameObject.Find("Canvas").transform;
            Transform = transform;
        }

        public abstract void Load();
    }
}
