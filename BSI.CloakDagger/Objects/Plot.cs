using System;
using System.Collections.Generic;
using BSI.CloakDagger.Helpers;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Objects
{
    public abstract class Plot
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TargetId { get; set; }

        public MBObjectBase Target => GameObjectHelper.GetGameObjectByStringId(TargetId);

        public string LeaderId { get; set; }

        public MBObjectBase Leader => GameObjectHelper.GetGameObjectByStringId(LeaderId);

        public List<string> MemberIds { get; set; }

        public List<MBObjectBase> Members => GameObjectHelper.GetGameObjectsByStringIds(MemberIds);

        public Goal ActiveGoal { get; set; }

        public Goal EndGoal { get; set; }

        public string TriggerTypeName { get; set; }

        public virtual void Initialize(string title, string description, MBObjectBase target, MBObjectBase leader, List<string> memberIds, Goal activeGoal, Goal endGoal, string triggerTypeName)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            TargetId = target.StringId;
            LeaderId = leader.StringId;
            MemberIds = new List<string>();
            MemberIds.AddRange(memberIds);
            ActiveGoal = activeGoal;
            EndGoal = endGoal;
            TriggerTypeName = triggerTypeName;
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