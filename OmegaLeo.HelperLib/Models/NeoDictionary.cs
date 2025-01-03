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
        
        public IEnumerable<NeoDictionaryItem<TKey, TValue>> Select(Func<NeoDictionaryItem<TKey, TValue>, NeoDictionaryItem<TKey, TValue>> selector) =>
            Items.Select(selector);

        public void Remove(TKey key)
        {
            var index = Items.FindIndex(x => x.Key.Equals(key));
            
            Items.RemoveAt(index);
        }

        public void RemoveAt(int index) => Items.RemoveAt(index);

        public void Replace(TKey key, TValue value)
        {
            if (HasKey(key))
            {
                this[key] = value;
            }
        }

        public bool HasKey(TKey key) => Items.Any(x => x.Key.Equals(key));
        
        public TValue this[TKey key]
        {
            get
            {
                if (Items == null)
                {
                    Items = new List<NeoDictionaryItem<TKey, TValue>>();
                    Items.Add(new NeoDictionaryItem<TKey, TValue>(key, (TValue)Activator.CreateInstance(typeof(TValue))));
                }
                
                try
                {
                    var value = Items.FirstOrDefault(x => EqualityComparer<TKey>.Default.Equals(x.Key, key))!.Value;

                    return value ?? (TValue)Activator.CreateInstance(typeof(TValue));
                }
                catch (Exception e)
                {
                    return (TValue)Activator.CreateInstance(typeof(TValue));
                }
            }
            set
            {
                if (HasKey(key))
                {
                    var item = Items.FirstOrDefault(x => EqualityComparer<TKey>.Default.Equals(x.Key, key));
                    var index = Items.IndexOf(item);
                    Items[index] = new NeoDictionaryItem<TKey, TValue>(key, value);
                }
                else
                {
                    Items.Add(new NeoDictionaryItem<TKey, TValue>(key, value));
                }
            }
        }

        public void Clear() => Items.Clear();
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