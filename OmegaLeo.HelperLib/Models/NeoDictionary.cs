using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetFlow.DocumentationHelper.Library.Attributes;

namespace GameDevLibrary.Models
{
    // Inspiration: https://www.youtube.com/watch?v=x-ejym1WdjE
    [Serializable]
    [Documentation("NeoDictionary", "Dictionary like class created to make it easier to display dictionaries in game engines like Unity")]
    public class NeoDictionary<TKey,TValue>
    {
        public List<NeoDictionaryItem<TKey, TValue>> Items = new List<NeoDictionaryItem<TKey, TValue>>();

        public Dictionary<TKey, TValue> ToDictionary() => (Items.Any())
            ? Items.ToDictionary(x => x.Key, x => x.Value)
            : new Dictionary<TKey, TValue>();

        public bool TryGetValue(TKey key, out TValue value)
        {
            var item = Items.FirstOrDefault(x => x.Key.Equals(key));
            
            if (item != null)
            {
                value = item.Value;
                return true;
            }

            value = default;
            
            return false;
        }
        
        public bool TryGetValueFromIndex(int index, out TValue value)
        {
            if (index < Items.Count)
            {
                var item = Items[index];

                if (item != null)
                {
                    value = item.Value;
                    return true;
                }
            }

            value = default;
            
            return false;
        }

        public void Add(TKey key, TValue value)
        {
            Items.Add(new NeoDictionaryItem<TKey, TValue>(key, value));
        }

        public void AddRange(NeoDictionary<TKey, TValue> values)
        {
            Items.AddRange(values.Items);
        }
        
        public void AddRange(IEnumerable<NeoDictionaryItem<TKey, TValue>> values)
        {
            Items.AddRange(values);
        }
        
        public bool Any() => Items.Any();

        public bool Any(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) => Items.Any(predicate);

        public IEnumerable<NeoDictionaryItem<TKey, TValue>> Where(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) =>
            Items.Where(predicate);

        public NeoDictionaryItem<TKey, TValue>? FirstOrDefault() => Items.FirstOrDefault();

        public NeoDictionaryItem<TKey, TValue>? FirstOrDefault(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) =>
            Items.FirstOrDefault(predicate);
        
        public NeoDictionaryItem<TKey, TValue>? LastOrDefault() => Items.LastOrDefault();

        public NeoDictionaryItem<TKey, TValue>? LastOrDefault(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) =>
            Items.LastOrDefault(predicate);

        public void Remove(TKey key)
        {
            var index = Items.FindIndex(x => x.Key.Equals(key));
            
            Items.RemoveAt(index);
        }

        public void RemoveAt(int index) => Items.RemoveAt(index);
    }

    [Serializable]
    public class NeoDictionaryItem<TKey,TValue>
    {
        public TKey Key;
        public TValue Value;

        public NeoDictionaryItem(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}