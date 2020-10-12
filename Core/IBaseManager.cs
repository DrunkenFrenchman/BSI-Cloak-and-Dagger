using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public interface IBaseManager<TKey, TValue> : IDictionary<String, TValue> where TValue : IBSIObjectBase
    { 
        TValue this[TKey key] { get; set; }
        new ICollection<TValue> Values { get; }
        void Add(TKey key, TValue value);
        bool ContainsKey(TKey key);
        bool Remove(TKey key);
        bool TryGetValue(TKey key, out TValue value);
        void AddItem(string stringId, IBSIObjectBase factionInfo);
    }
}
