using System.Collections.Generic;
using UnityEngine;
namespace Frame {
    public static class UsefulFunctions {
        public static void DestroyAllChildren(Transform parent) {
            foreach (Transform t in parent) {
                GameObject.Destroy(t.gameObject);
            }
        }
        /// <summary>
        /// 判断数组是否包含某一项元素
        /// </summary>
        /// <typeparam name="T">任意</typeparam>
        /// <param name="arr">数组</param>
        /// <param name="it">元素</param>
        /// <returns>是否包含</returns>
        public static bool ArrayContains<T>(T[] arr, T it) {
            if (arr.Length == 0) return true;
            for (int i = 0; i < arr.Length; i++) { 
                if (EqualityComparer<T>.Default.Equals(arr[i], it)) return true;
            }
            return false;
        }
    }
}