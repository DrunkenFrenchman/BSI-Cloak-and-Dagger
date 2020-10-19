using BSI.Core.Enumerations;
using System;
using TaleWorlds.ObjectSystem;

namespace BSI.Core.Objects
{
    public abstract class Trigger
    {
        public Trigger()
        {
            this.Id = Guid.NewGuid();
            this.UniqueTo = UniqueTo.NotSet;
            this.AllowedInstancesPerGameObject = 1;
        }

        public Guid Id { get; set; }

        public UniqueTo UniqueTo { get; set; }

        public int AllowedInstancesPerGameObject { get; set; }

        public abstract bool CanStart(MBObjectBase gameObject);

        public abstract Plot Start(MBObjectBase gameObject);
    }
}