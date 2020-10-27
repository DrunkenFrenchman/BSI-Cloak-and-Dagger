using BSI.CloakDagger.Enumerations;
using System;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Objects
{
    public abstract class Trigger
    {
        public string Id { get; set; }

        public UniqueTo UniqueTo { get; set; }

        public int AllowedInstances { get; set; }

        public abstract bool CanStart(MBObjectBase gameObject);

        public abstract Plot DoStart(MBObjectBase gameObject);

        public virtual void Initialize(UniqueTo uniqueTo, int allowedInstances)
        {
            Id = Guid.NewGuid().ToString();
            UniqueTo = uniqueTo;
            AllowedInstances = allowedInstances;
        }
    }
}