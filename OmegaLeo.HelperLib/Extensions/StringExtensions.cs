using System;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Extensions
{
    [Documentation(nameof(StringExtensions), "Provides extension methods for the string data type.", null,
        "")]
    [Changelog("1.2.0", "Fixed root namespace to OmegaLeo.HelperLib.Extensions.", "January 28, 2026")]
    public static class StringExtensions
    {
        /// <summary>
        /// Cut off a string once it reaches a <paramref name="maxChars"/> amount of characters and add '...' to the end of the string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        [Documentation(nameof(Truncate),
            "Truncates the string to a specified maximum number of characters, appending '...' if truncation occurs.",
            new[]
            {
                "value: The string to truncate.",
                "maxChars: The maximum number of characters allowed before truncation."
            },
            @"```cs
var longString = ""This is a very long string that needs to be truncated."";
var truncatedString = longString.Truncate(20); // truncatedString will be ""This is a very long...""
```")]
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
        
        /// <summary>
        /// Identify if a string is null or empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [Documentation(nameof(IsNullOrEmpty),
            "Checks if the string is null or empty.",
            new[] { "str: The string to check." },
            @"```cs
string myString = """";
bool isNullOrEmpty = myString.IsNullOrEmpty(); // isNullOrEmpty will be true
```")]
        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }
        
        /// <summary>
        /// Identify if a string is <b>not</b> null or empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [Documentation(nameof(IsNotNullOrEmpty),
            "Checks if the string is not null or empty.",
            new[] { "str: The string to check." },
            @"```cs
string myString = ""Hello"";
bool isNotNullOrEmpty = myString.IsNotNullOrEmpty(); // isNotNullOrEmpty will be true
```")]
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !str.IsNullOrEmpty();
        }

        /// <summary>
        /// Check if the string matches any of the search parameters
        /// </summary>
        /// <param name="str"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [Documentation(nameof(AnyMatch),
            "Checks if the string matches any of the provided search strings, ignoring case.",
            new[]
            {
                "str: The string to check.",
                "search: An array of strings to match against."
            },
            @"```cs
string myString = ""Hello"";
bool matches = myString.AnyMatch(""hi"", ""hello"", ""greetings""); // matches will be true
```")]
        public static bool AnyMatch(this string str, params string[] search)
        {
            foreach (string s in search)
            {
                if (s.Equals(str, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Reverses a given string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [Documentation(nameof(Reverse),
            "Reverses the characters in the string.",
            new[] { "str: The string to reverse." },
            @"```cs
string myString = ""Hello"";
string reversedString = myString.Reverse(); // reversedString will be ""olleH""
```")]
        public static string Reverse(this string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}