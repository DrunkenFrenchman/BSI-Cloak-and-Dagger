using System;
using Newtonsoft.Json;

namespace BSI.CloakDagger.Objects
{
    public abstract class Goal
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public Behavior Behavior { get; set; }

        public Goal NextGoal { get; set; }

        [JsonIgnore]
        public Plot Plot { get; set; }

        public virtual void Initialize(string title, string description, Goal nextGoal, Plot plot)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            NextGoal = nextGoal;
            Plot = plot;
        }
    }
}