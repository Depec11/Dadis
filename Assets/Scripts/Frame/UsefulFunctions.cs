using UnityEngine;

namespace Frame {
    public static class UsefulFunctions {
        public static void DestroyAllChildren(Transform parent) {
            foreach (Transform t in parent) {
                GameObject.Destroy(t.gameObject);
            }
        }
    }
}