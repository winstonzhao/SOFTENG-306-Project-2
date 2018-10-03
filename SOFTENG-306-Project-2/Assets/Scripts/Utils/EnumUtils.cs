using System;

namespace Utils
{
    public class EnumUtils
    {
        private EnumUtils()
        {
        }

        public static string[] GetNames<T>()
        {
            return Enum.GetNames(typeof(T));
        }

        public static T[] GetValues<T>()
        {
            return (T[]) Enum.GetValues(typeof(T));
        }
    }
}
