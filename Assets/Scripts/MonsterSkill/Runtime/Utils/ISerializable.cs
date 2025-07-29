using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MonsterSkill.Runtime
{
    public interface ISerializable<T> where T : ISerializable<T>
    {
        /// <summary>
        /// 获取可序列化类型
        /// </summary>
        public static Type Type => typeof(T);

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
            {
#if DEBUG
                LogUnserializableContent(Type);
#endif
                return null;
            }

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

        #region Helper

        private static void LogUnserializableContent(Type type)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine($"Type {type.FullName} cannot be serialized. Possible issues:");

            // 检查属性[Serializable]
            if (!type.IsDefined(typeof(SerializableAttribute), false))
                errorMessage.AppendLine($"- Type {type.FullName} is missing the [Serializable] attribute.");

            // 检查字段
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                // 跳过[NonSerializable]
                if (field.IsDefined(typeof(NonSerializedAttribute), false))
                    continue;

                var fieldType = field.FieldType;
                if (!IsTypeSerializable(fieldType))
                    errorMessage.AppendLine(
                        $"- Field '{field.Name}' of type {fieldType.FullName} is not serializable by JsonUtility.");
            }

            // 检查属性，JSONUtility不会也不应该序列化属性，记得打上Tag
            var properties = type.GetProperties(BindingFlags.Public |
                                                BindingFlags.NonPublic |
                                                BindingFlags.Instance);
            foreach (var property in properties)
            {
                // 跳过[NonSerializable]
                if (property.IsDefined(typeof(NonSerializedAttribute), false))
                    continue;

                if (property.CanRead && property.CanWrite) // Only consider properties with getters and setters
                    errorMessage.AppendLine(
                        $"- Property '{property.Name}' of type {property.PropertyType.FullName} is present. JsonUtility does not serialize properties.");
            }

            Debug.LogError(errorMessage.ToString());
        }

        private static bool IsTypeSerializable(Type type)
        {
            // Primitive types, strings, and enums are generally serializable
            if (type.IsPrimitive || type == typeof(string) || type.IsEnum)
                return true;

            // Unity-specific types like Vector2, Vector3, etc., are serializable
            if (type.Namespace != null && type.Namespace.StartsWith("UnityEngine") && type.IsSerializable)
                return true;

            // Arrays and lists are serializable if their element types are serializable
            if (type.IsArray)
                return IsTypeSerializable(type.GetElementType());

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return IsTypeSerializable(type.GetGenericArguments()[0]);

            // Check if the type is marked as [Serializable] and not abstract
            if (type.IsSerializable && !type.IsAbstract)
            {
                // Recursively check fields of the type
                var fields = type.GetFields(BindingFlags.Public |
                                            BindingFlags.NonPublic |
                                            BindingFlags.Instance);
                return fields.All(field =>
                    field.IsDefined(typeof(NonSerializedAttribute), false) || IsTypeSerializable(field.FieldType));
            }

            return false;
        }

        #endregion
    }
}