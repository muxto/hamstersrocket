using System;

namespace HamstersRocket.Core.Helpers
{
    public static class Extenstions
    {
        public static T[] Slice<T>(this T[] source, int index, int length)
        {
            if (index >= source.Length) return new T[0]; 
            if ((index + length) >= source.Length)
            {
                length = source.Length - index;
            }

            T[] slice = new T[length];
            Array.Copy(source, index, slice, 0, length);
            return slice;
        }
    }
}
