﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace GameDevLibrary.Extensions
{
    public static class IListExtensions
    {
        /// <summary>
        /// Swaps the position of 2 elements inside of a List
        /// </summary>
        /// <param name="list"></param>
        /// <param name="indexA"></param>
        /// <param name="indexB"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>List with swapped elements</returns>
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
            return list;
        }
        
        /// <summary>
        /// Swaps the position of 2 elements inside of a List
        /// </summary>
        /// <param name="list"></param>
        /// <param name="itemA"></param>
        /// <param name="itemB"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>List with swapped elements</returns>
        public static IList<T> Swap<T>(this IList<T> list, T itemA, T itemB)
        {
            return list.Swap(list.IndexOf(itemA), list.IndexOf(itemB));
        }
        
        /// <summary>
        /// Replace an element inside a list with a different element
        /// </summary>
        /// <param name="list"></param>
        /// <param name="originalValue"></param>
        /// <param name="valueToReplace"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> Replace<T>(this IList<T> list, T originalValue, T valueToReplace)
        {
            var index = list.IndexOf(originalValue);

            if (index != -1)
            {
                list[index] = valueToReplace;
            }

            return list;
        }

        /// <summary>
        /// Method to obtain a random element from inside a list
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Random<T>(this IList<T> list)
        {
            if (list.Count == 0) return default;

            // Updated based on Robin King's tip about random items https://twitter.com/quoxel/status/1729137730607841755/photo/1
            int seed = (int)DateTime.Now.Ticks;
            
            var r = new Random(seed);

            var randomIndex = r.Next(0, list.Count);

            var returnValue = list[randomIndex];

            return returnValue ?? list[0];

        }
    
        /// <summary>
        /// Method to obtain <paramref name="count"/> elements from inside a list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static List<T> Random<T>(this IList<T> list, int count = 1)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            
            List<T> values = new List<T>();

            for (int i = 0; i < count; i++)
            {
                values.Add(list.Where(x => !values.Contains(x)).ToList().Random());
            }
        
            return values;
        }
        
        /*public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }*/

        
        // Idea obtained from TaroDev's video on things to do in Unity - https://youtu.be/Ic5ux-tpkCE?t=302
        public static T Next<T>(this List<T> array, ref int currentIndex)
        {
            return array[currentIndex++ % array.Count];
        }
        
        /// <summary>
        /// Shuffle around items in a list.
        /// Code obtained from https://stackoverflow.com/a/1262619
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                (list[k], list[n]) = (list[n], list[k]);
            }

            return list;
        }

        
    }
}