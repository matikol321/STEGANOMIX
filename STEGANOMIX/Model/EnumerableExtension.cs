using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEGANOMIX.Model
{
    public static class EnumerableExtension
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            for(var i=0; i<(float)array.Length / size; i++)
            {
                yield return array.Skip(i*size).Take(size);
            }
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this List<T> list, int size)
        {
            for (var i = 0; i < (float)list.Count / size; i++)
            {
                yield return list.Skip(i * size).Take(size);
            }
        }
    }
}
