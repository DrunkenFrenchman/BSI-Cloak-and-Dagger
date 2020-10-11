using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public abstract class BaseManager<TKey, TFaction> : IBaseManager<String, FactionInfo> where TFaction : FactionInfo
    {
        public FactionInfo this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ICollection<FactionInfo> Values => throw new NotImplementedException();

        public ICollection<string> Keys => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(string key, FactionInfo value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, FactionInfo> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, FactionInfo> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, FactionInfo>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, FactionInfo>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, FactionInfo> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out FactionInfo value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
