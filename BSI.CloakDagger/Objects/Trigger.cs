using BSI.CloakDagger.Enumerations;
using System;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Objects
{
    public abstract class Trigger
    {
        public Trigger()
        {
            Id = Guid.NewGuid();
            UniqueTo = UniqueTo.NotSet;
            AllowedInstancesPerGameObject = 0;
        }

        public Guid Id { get; set; }

        public UniqueTo UniqueTo { get; set; }

        public int AllowedInstancesPerGameObject { get; set; }

        public abstract bool CanStart(MBObjectBase gameObject);

        public abstract Plot Start(MBObjectBase gameObject);
    }
}