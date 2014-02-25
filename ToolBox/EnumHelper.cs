using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolBox
{
    /// <summary>
    /// Some method to facilitate the use of enum
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Convert the array of enum to array of integer.
        /// </summary>
        /// <typeparam name="T">type of enum</typeparam>
        /// <param name="value">Array of enum</param>
        /// <returns>Array of integer</returns>
        public static int[] ToIntArray<T>(T[] value)
        {
            int[] result = new int[value.Length];
            for (int i = 0; i < value.Length; i++)
                result[i] = Convert.ToInt32(value[i]);
            return result;
        }

        /// <summary>
        /// Convert a array of integer to array of enum
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">Array of integer</param>
        /// <returns>Array of enum</returns>
        public static T[] FromIntArray<T>(int[] value)
        {
            T[] result = new T[value.Length];
            for (int i = 0; i < value.Length; i++)
                result[i] = (T)Enum.ToObject(typeof(T), value[i]);
            return result;
        }

        /// <summary>
        /// Parse string to enum. If method fail, return default value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String enum value.</param>
        /// <param name="defaultValue">Return if method fail.</param>
        /// <returns>Enum value</returns>
        public static T Parse<T>(string value, T defaultValue)
        {
            if (Enum.IsDefined(typeof(T), value))
                return (T)Enum.Parse(typeof(T), value);

            int num;
            if (int.TryParse(value, out num))
            {
                if (Enum.IsDefined(typeof(T), num))
                    return (T)Enum.ToObject(typeof(T), num);
            }

            return defaultValue;
        }
    }
}
