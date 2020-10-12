using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public abstract class BaseManager<TKey, TValue> : IBaseManager<String, FactionInfo<IFaction>>
    {
        public FactionInfo<IFaction> this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ICollection<FactionInfo<IFaction>> Values => throw new NotImplementedException();

        public ICollection<string> Keys => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(string key, FactionInfo<IFaction> value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, FactionInfo<IFaction>> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, FactionInfo<IFaction>> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, FactionInfo<IFaction>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, FactionInfo<IFaction>>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, FactionInfo<IFaction>> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out FactionInfo<IFaction> value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
