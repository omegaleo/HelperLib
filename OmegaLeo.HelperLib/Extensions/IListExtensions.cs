using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Extensions
{
    [Documentation(nameof(IListExtensions),
        "Extension methods for IList<T> to provide additional functionalities such as swapping, replacing, random selection, and shuffling.", 
        null, 
        "")]
    [Changelog("1.2.0", "Fixed root namespace to OmegaLeo.HelperLib.Extensions.", "January 28, 2026")]
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
        [Documentation(nameof(Swap),
            "Swaps the elements at the specified indices in the list.", 
            new[]
            {
                "list: The list in which to swap elements.",
                "indexA: The index of the first element to swap.",
                "indexB: The index of the second element to swap."
            }, 
            @"```cs
var myList = new List<int> { 1, 2, 3, 4 };
myList.Swap(0, 2); // myList is now { 3, 2, 1, 4 }
```
")]
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
        [Documentation(nameof(Swap),
            "Swaps the elements at the specified indices in the list.", 
            new[]
            {
                "list: The list in which to swap elements.",
                "indexA: The index of the first element to swap.",
                "indexB: The index of the second element to swap."
            }, 
            @"```cs
var myList = new List<int> { 1, 2, 3, 4 };
myList.Swap(0, 2); // myList is now { 3, 2, 1, 4 }
```
")]
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
        [Documentation(nameof(Replace),
            "Replaces the first occurrence of a specified value in the list with a new value.", 
            new[]
            {
                "list: The list in which to replace an element.",
                "originalValue: The value to be replaced.",
                "valueToReplace: The new value to insert."
            }, 
            @"```cs
var myList = new List<int> { 1, 2, 3, 4 };
myList.Replace(2, 5); // myList is now { 1, 5, 3, 4 }
```")]
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
        [Documentation(nameof(Random),
            "Selects a random element from the list.", 
            new[]
            {
                "list: The list from which to select a random element."
            }, 
            @"```cs
var myList = new List<int> { 1, 2, 3, 4 };
var randomElement = myList.Random(); // randomElement could be any of 1, 2, 3, or 4
```")]
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
        [Documentation(nameof(Random),
            "Selects a specified number of unique random elements from the list.", 
            new[]
            {
                "list: The list from which to select random elements.",
                "count: The number of unique random elements to select."
            }, 
            @"```cs
var myList = new List<int> { 1, 2, 3, 4 };
var randomElements = myList.Random(2); // randomElements could be any two of 1, 2, 3, or 4
```")]
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
        
        // Idea obtained from TaroDev's video on things to do in Unity - https://youtu.be/Ic5ux-tpkCE?t=302
        [Documentation("Next<T>", @"Method to obtain the next object in the list by passing the current index.",
            new []
        {
            "CurrentIndex - The index we're currently on inside the list passed as reference so we can automatically assign to it the next index"
        },
            @"```cs
var myList = new List<int> { 1, 2, 3,4 };
var currentIndex = 0;
var nextItem = myList.Next(ref currentIndex); // nextItem will be 1, currentIndex will be 1
var nextItem2 = myList.Next(ref currentIndex); // nextItem2 will be 2, currentIndex will be 2
```
")]
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
        [Documentation("Shuffle<T>",
            @"Shuffles the elements of the list in place using a cryptographic random number generator.  
  
Code obtained from https://stackoverflow.com/a/1262619", 
            new[]
            {
                "list: The list to be shuffled."
            }, 
            @"```cs
var myList = new List<int> { 1, 2, 3, 4 };
myList.Shuffle(); // myList could now be { 3, 1, 4, 2 } (example output)
```")]
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