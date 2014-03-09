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
        /// <exception cref="FormatException">If one value is not in an appropriate format.</exception>
        /// <exception cref="InvalidCastException">The conversion is not supported.</exception>
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
        /// <exception cref="ArgumentException">one value is not in enum.</exception>
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
        /// <exception cref="ArgumentException">T type is not a enum.</exception>
        public static T Parse<T>(string value, T defaultValue)
        {
            if (value != null && Enum.IsDefined(typeof(T), value))
                return (T)Enum.Parse(typeof(T), value);

            return defaultValue;
        }

        /// <summary>
        /// Converts the string representation of a enum equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">A string containing a enum value to convert</param>
        /// <param name="result">Enum value, if the convert is successed</param>
        /// <returns>true if was converted successfully; otherwise, false</returns>
        /// <exception cref="ArgumentException">T type is not a enum.</exception>
        public static bool TryParse<T>(string value, ref T result)
        {
            if (value != null && Enum.IsDefined(typeof(T), value))
            {
                result = (T)Enum.Parse(typeof(T), value);
                return true;
            }
            return false;
        }
    }
}
