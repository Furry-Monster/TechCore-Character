using System;
using UnityEngine;

namespace MonsterSkill.Runtime
{
    public interface ISerializable<T> where T : ISerializable<T>
    {
        public Type Type => typeof(T);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">被序列化的类型</param>
        /// <returns>序列化之后的json字符串</returns>
        public static string Serialize(T obj)
        {
            if (obj == null)
                return null;

            if (!obj.GetType().IsSerializable)
                return null;

            var json = JsonUtility.ToJson(obj);
            return json;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json">存储的json字符串</param>
        /// <returns>反序列化得到的对应类型实例</returns>
        public static T Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default;

            var obj = JsonUtility.FromJson<T>(json);
            return obj;
        }
    }
}