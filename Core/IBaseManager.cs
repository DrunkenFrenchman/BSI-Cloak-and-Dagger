using BSI.CivilWar.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CivilWar.Core
{
    public interface IBaseManager<TKey, TValue> : IDictionary<String, FactionInfo>
    {
        TValue this[TKey key] { get; set; }
        new ICollection<TValue> Values { get; }
        void Add(TKey key, TValue value);
        bool ContainsKey(TKey key);
        bool Remove(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }
}
