using System;
using BSI.CloakDagger.Enumerations;

namespace BSI.CloakDagger.Models.PlotMod
{
    public abstract class Trigger
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public GameObjectType UniqueToType { get; set; }

        public GameObjectType InitiatorType { get; set; }

        public int AllowedInstances { get; set; }

        public abstract bool CanStart(GameObject gameObject);

        public abstract bool CanPlayerStart();

        public abstract Plot DoStart(GameObject gameObject);

        public virtual void Initialize(GameObjectType uniqueToType, string title, string description, GameObjectType initiatorType, int allowedInstances)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            UniqueToType = uniqueToType;
            InitiatorType = initiatorType;
            AllowedInstances = allowedInstances;
        }
    }
}