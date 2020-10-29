using System;
using BSI.CloakDagger.Enumerations;

namespace BSI.CloakDagger.Models.PlotMod
{
    public abstract class Trigger
    {
        public string Id { get; set; }

        public GameObjectType UniqueToType { get; set; }

        public GameObjectType InitiatorType { get; set; }

        public int AllowedInstances { get; set; }

        public abstract bool CanStart(GameObject gameObject);

        public abstract Plot DoStart(GameObject gameObject);

        public virtual void Initialize(GameObjectType uniqueToType, GameObjectType initiatorType, int allowedInstances)
        {
            Id = Guid.NewGuid().ToString();
            UniqueToType = uniqueToType;
            InitiatorType = initiatorType;
            AllowedInstances = allowedInstances;
        }
    }
}