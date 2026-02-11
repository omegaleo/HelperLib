using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Models
{
    // Inspiration: https://www.youtube.com/watch?v=x-ejym1WdjE
    [Serializable]
    [Documentation("NeoDictionary", "Dictionary like class created to make it easier to display dictionaries in game engines like Unity")]
    [Changelog("1.2.0", "Fixed root namespace to OmegaLeo.HelperLib.Models.", "January 28, 2026")]
    public class NeoDictionary<TKey,TValue>
    {
        public List<NeoDictionaryItem<TKey, TValue>> Items = new List<NeoDictionaryItem<TKey, TValue>>();

        public Dictionary<TKey, TValue> ToDictionary() => (Items.Any())
            ? Items.ToDictionary(x => x.Key, x => x.Value)
            : new Dictionary<TKey, TValue>();

        public static implicit operator Dictionary<TKey, TValue>(NeoDictionary<TKey, TValue> dict) => (dict.Items.Any())
            ? dict.Items.ToDictionary(x => x.Key, x => x.Value)
            : new Dictionary<TKey, TValue>();
        
        [Documentation("TryGetValue", "Tries to get the value from the NeoDictionary for the given key.")]
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
        
        [Documentation("TryGetValueFromIndex", "Tries to get the value from the NeoDictionary at the given index.")]
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

        [Documentation("Add", "Adds a new NeoDictionaryItem to the NeoDictionary.")]
        public void Add(TKey key, TValue value)
        {
            Items.Add(new NeoDictionaryItem<TKey, TValue>(key, value));
        }

        [Documentation("AddRange", "Adds a range of NeoDictionaryItems from another NeoDictionary to the NeoDictionary.")]
        public void AddRange(NeoDictionary<TKey, TValue> values)
        {
            Items.AddRange(values.Items);
        }
        
        [Documentation("AddRange", "Adds a range of NeoDictionaryItems to the NeoDictionary.")]
        public void AddRange(IEnumerable<NeoDictionaryItem<TKey, TValue>> values)
        {
            Items.AddRange(values);
        }
        
        [Documentation("Any", "Checks if the NeoDictionary has any items.")]
        public bool Any() => Items.Any();

        [Documentation("Any", "Checks if any item in the NeoDictionary satisfies a condition.")]
        public bool Any(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) => Items.Any(predicate);

        [Documentation("Where", "Filters the NeoDictionary items based on a predicate.")]
        public IEnumerable<NeoDictionaryItem<TKey, TValue>> Where(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) =>
            Items.Where(predicate);

        [Documentation("FirstOrDefault", "Returns the first item of the NeoDictionary or a default value if the NeoDictionary is empty.")]
        public NeoDictionaryItem<TKey, TValue>? FirstOrDefault() => Items.FirstOrDefault();

        [Documentation("FirstOrDefault", "Returns the first item of the NeoDictionary that satisfies a condition or a default value if no such item is found.")]
        public NeoDictionaryItem<TKey, TValue>? FirstOrDefault(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) =>
            Items.FirstOrDefault(predicate);
        
        [Documentation("LastOrDefault", "Returns the last item of the NeoDictionary or a default value if the NeoDictionary is empty.")]
        public NeoDictionaryItem<TKey, TValue>? LastOrDefault() => Items.LastOrDefault();

        [Documentation("LastOrDefault", "Returns the last item of the NeoDictionary that satisfies a condition or a default value if no such item is found.")]
        public NeoDictionaryItem<TKey, TValue>? LastOrDefault(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) =>
            Items.LastOrDefault(predicate);
        
        [Documentation("Select", "Projects each item of the NeoDictionary into a new form.")]
        public IEnumerable<NeoDictionaryItem<TKey, TValue>> Select(Func<NeoDictionaryItem<TKey, TValue>, NeoDictionaryItem<TKey, TValue>> selector) =>
            Items.Select(selector);
        
        [Documentation("IndexOf", "Returns the index of the given NeoDictionaryItem.")]
        public int IndexOf(NeoDictionaryItem<TKey, TValue> selector) =>
            Items.IndexOf(selector);

        [Documentation("Remove", "Removes the item with the given key.")]
        public void Remove(TKey key)
        {
            var index = Items.FindIndex(x => x.Key.Equals(key));
            
            Items.RemoveAt(index);
        }

        [Documentation("RemoveAt", "Removes the item at the given index.")]
        public void RemoveAt(int index) => Items.RemoveAt(index);

        [Documentation("Replace", "Replaces the value for the given key if it exists.")]
        public void Replace(TKey key, TValue value)
        {
            if (HasKey(key))
            {
                this[key] = value;
            }
        }

        [Documentation("HasKey", "Checks if the NeoDictionary contains the given key.")]
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

        public NeoDictionaryItem<TKey, TValue> this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }

        [Documentation("Clear", "Clears all items from the NeoDictionary.")]
        public void Clear() => Items.Clear();
        
        [Documentation("Sort", "Sorts the NeoDictionary items using the given comparison.")]
        public void Sort(Comparison<NeoDictionaryItem<TKey, TValue>> comparison) => Items.Sort(comparison);
        
        [Documentation("Count", "Returns the number of items in the NeoDictionary.")]
        public int Count() => Items.Count;
        
        [Documentation("ForEach", "Performs the given action on each item in the NeoDictionary.")]
        public void ForEach(Action<NeoDictionaryItem<TKey, TValue>> action) => Items.ForEach(action);
        
        [Documentation("ToList", "Converts the NeoDictionary items to a List.")]
        public List<NeoDictionaryItem<TKey, TValue>> ToList() => Items.ToList();
        
        [Documentation("Clone", "Creates a shallow copy of the NeoDictionary.")]
        public NeoDictionary<TKey, TValue> Clone()
        {
            var newDict = new NeoDictionary<TKey, TValue>();
            newDict.Items = new List<NeoDictionaryItem<TKey, TValue>>(Items);
            return newDict;
        }
        
        [Documentation("Reverse", "Reverses the order of the items in the NeoDictionary.")]
        public void Reverse() => Items.Reverse();
        
        [Documentation("Insert", "Inserts an item at the given index.")]
        public void Insert(int index, TKey key, TValue value) => 
            Items.Insert(index, new NeoDictionaryItem<TKey, TValue>(key, value));
        
        [Documentation("InsertRange", "Inserts a range of items starting from the given index.")]
        public void InsertRange(int index, IEnumerable<NeoDictionaryItem<TKey, TValue>> values) => 
            Items.InsertRange(index, values);
        
        [Documentation("RemoveRange", "Removes a range of items starting from the given index.")]
        public void RemoveRange(int index, int count) => 
            Items.RemoveRange(index, count);
        
        [Documentation("RemoveAll", "Removes all items matching the given predicate.")]
        public void RemoveAll(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) => 
            Items.RemoveAll(new Predicate<NeoDictionaryItem<TKey, TValue>>(predicate));
        
        [Documentation("FindIndex", "Finds the index of the first item matching the given predicate.")]
        public int FindIndex(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) => 
            Items.FindIndex(new Predicate<NeoDictionaryItem<TKey, TValue>>(predicate));
        
        [Documentation("Find", "Finds the first item matching the given predicate.")]
        public NeoDictionaryItem<TKey, TValue> Find(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) => 
            Items.Find(new Predicate<NeoDictionaryItem<TKey, TValue>>(predicate));
        
        [Documentation("FindAll", "Finds all items matching the given predicate.")]
        public IEnumerable<NeoDictionaryItem<TKey, TValue>> FindAll(Func<NeoDictionaryItem<TKey, TValue>, bool> predicate) => 
            Items.FindAll(new Predicate<NeoDictionaryItem<TKey, TValue>>(predicate));
        
        [Documentation("Merge", "Merges another NeoDictionary into this one, ignoring duplicate keys.")]
        public void Merge(NeoDictionary<TKey, TValue> other)
        {
            foreach (var item in other.Items)
            {
                if (!HasKey(item.Key))
                {
                    Add(item.Key, item.Value);
                }
            }
        }
        
        [Documentation("ToDictionarySafe", "Converts the NeoDictionary to a standard Dictionary while safely handling duplicate keys by ignoring them.")]
        public Dictionary<TKey, TValue> ToDictionarySafe()
        {
            var dict = new Dictionary<TKey, TValue>();
            
            foreach (var item in Items)
            {
                if (!dict.ContainsKey(item.Key))
                {
                    dict.Add(item.Key, item.Value);
                }
            }

            return dict;
        }
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