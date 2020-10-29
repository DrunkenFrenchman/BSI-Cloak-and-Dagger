using System;
using System.Collections.Generic;

namespace BSI.CloakDagger.Models.PlotMod
{
    public abstract class Plot
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public GameObject Target { get; set; }

        public GameObject Leader { get; set; }

        public List<GameObject> Members { get; set; }

        public Goal ActiveGoal { get; set; }

        public Goal EndGoal { get; set; }

        public string TriggerType { get; set; }

        public virtual void Initialize(string title, string description, GameObject target, GameObject leader, List<GameObject> members, Goal activeGoal, Goal endGoal, string triggerType)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            Target = target;
            Leader = leader;
            Members = new List<GameObject>();
            Members.AddRange(members);
            ActiveGoal = activeGoal;
            EndGoal = endGoal;
            TriggerType = triggerType;
        }

        public bool IsEndGoal()
        {
            return ActiveGoal == EndGoal;
        }

        public void SetNextGoal()
        {
            ActiveGoal = ActiveGoal.NextGoal ?? EndGoal;
        }

        public virtual bool CanAbort()
        {
            return Members.Count == 0;
        }

        public virtual void DoAbort()
        {
        }
    }
}