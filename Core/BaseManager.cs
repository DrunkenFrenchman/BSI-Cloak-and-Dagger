using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public abstract class BaseManager<TKey, TValue> : IBaseManager<String, FactionInfo<IFaction>>
    {
        public FactionInfo<IFaction> this[string key] { get => this[key] ; set => this[key] = value; }

        public ICollection<FactionInfo<IFaction>> Values => this.Values;

        public ICollection<string> Keys => this.Keys;

        public int Count => this.Count;

        public bool IsReadOnly => this.IsReadOnly;

        public void Add(string key, FactionInfo<IFaction> value)
        {
            this.Keys.Add(key);
            this.Values.Add(value);
        }

        public void Add(KeyValuePair<string, FactionInfo<IFaction>> item)
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

        public bool Contains(KeyValuePair<string, FactionInfo<IFaction>> item)
        {
            return this.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return this.Keys.Contains(key);
        }

        public void CopyTo(KeyValuePair<string, FactionInfo<IFaction>>[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, FactionInfo<IFaction>>> GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return this.Remove(key);
        }

        public bool Remove(KeyValuePair<string, FactionInfo<IFaction>> item)
        {
            return this.Remove(item);
        }

        public bool TryGetValue(string key, out FactionInfo<IFaction> value)
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
