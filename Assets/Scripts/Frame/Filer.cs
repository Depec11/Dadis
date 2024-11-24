using System;
using UnityEngine;

namespace Frame {
    /// <summary>
    /// 文件处理
    /// </summary>
    public static class Filer {
        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="path">Resources中的文件路径</param>
        /// <param name="startLine">开始读取的行数</param>
        /// <param name="split">分隔符</param>
        public static string[,] ReadCSV(string path, int startLine = 0, string split = ";", string holder = "NONE") {
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            string str = textAsset.text.Replace("\r", "");
            string[] t = str.Split("\n");
            int l = t[0].Split(split).Length;
            string[,] res = new string[t.Length - startLine, l];
            for (int i = startLine; i < t.Length; i++) {
                string[] tt = t[i].Split(split);
                for (int j = 0; j < tt.Length; j++) { res[i - startLine, j] = tt[j] == "" || tt[j] == "\r" || tt[j] == string.Empty ? holder : tt[j]; }
            }
            return res;
        }
        /// <summary>
        /// 字符串批量转枚举类
        /// </summary>
        /// <typeparam name="T">枚举类</typeparam>
        /// <param name="str">字符串数组</param>
        /// <param name="split">分隔符</param>
        public static T[] StringsToEnum<T>(string str, string split = " ") where T : Enum { 
            string[] t = str.Split(split);
            T[] res = new T[t.Length];
            for (int i = 0; i < t.Length; i++) { res[i] = StringToEnum<T>(t[i]); }
            return res;
        }
        /// <summary>
        /// 字符串转枚举类
        /// </summary>
        /// <typeparam name="T">枚举类</typeparam>
        /// <param name="str">字符串</param>
        public static T StringToEnum<T>(string str) {
            return (T)Enum.Parse(typeof(T), str);
        }
        public static T Parse<T>(string str) where T: IConvertible {
            try {
                object parsed = Convert.ChangeType(str, typeof(T));
                return (T)parsed;
            } catch { return default; }
        }
    }
}
