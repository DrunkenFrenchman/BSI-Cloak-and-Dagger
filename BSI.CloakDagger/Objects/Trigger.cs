using System;
using BSI.CloakDagger.Enumerations;

namespace BSI.CloakDagger.Objects
{
    public abstract class Trigger
    {
        public string Id { get; set; }

        public GameObjectType UniqueTo { get; set; }

        public int AllowedInstances { get; set; }

        public abstract bool CanStart(GameObject gameObject);

        public abstract Plot DoStart(GameObject gameObject);

        public virtual void Initialize(GameObjectType uniqueTo, int allowedInstances)
        {
            Id = Guid.NewGuid().ToString();
            UniqueTo = uniqueTo;
            AllowedInstances = allowedInstances;
        }
    }
}