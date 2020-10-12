using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public abstract class BaseManager<TKey, TValue> : IBaseManager<String, IBSIObjectBase>
    {
        public IBSIObjectBase this[string key] { get => this[key] ; set => this[key] = value; }

        public ICollection<IBSIObjectBase> Values => this.Values;

        public ICollection<string> Keys => this.Keys;

        public int Count => this.Count;

        public bool IsReadOnly => this.IsReadOnly;

        public void Add(string key, IBSIObjectBase value)
        {
            this.Keys.Add(key);
            this.Values.Add(value);
        }

        public void Add(KeyValuePair<string, IBSIObjectBase> item)
        {
            this.AddItem(item);
        }

        public void AddItem(string stringId, FactionInfo<Clan> factionInfo)
        {
            this.AddItem(stringId, factionInfo);
        }

        public void Clear()
        {
            this.Clear();
        }

        public bool Contains(KeyValuePair<string, IBSIObjectBase> item)
        {
            return this.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return this.Keys.Contains(key);
        }

        public void CopyTo(KeyValuePair<string, IBSIObjectBase>[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, IBSIObjectBase>> GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return this.Remove(key);
        }

        public bool Remove(KeyValuePair<string, IBSIObjectBase> item)
        {
            return this.Remove(item);
        }

        public bool TryGetValue(string key, out IBSIObjectBase value)
        {
            if (this.Keys.Contains(key)) 
            {
                value = this[key];
                return true;
            }
            value = null;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
