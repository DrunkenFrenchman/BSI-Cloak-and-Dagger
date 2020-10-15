using BSI.Core;
using BSI.Core.Flags;
using BSI.Core.Managers;
using BSI.Core.Objects;
using BSI.Core.Tools;
using System;
using TaleWorlds.CampaignSystem;

namespace BSI.Plots.CivilWar
{
    public class CivilWar : Plot
    {         

        public CivilWar(Hero instigator, Goal endGoal, IFaction target, Goal initialGoal = null, Uniqueto uniqueto = Uniqueto.Kingdom) : base(instigator, endGoal, target, initialGoal)
        {

            if (instigator.Clan.Leader.Equals(instigator) && !instigator.Clan.Kingdom.Leader.Equals(instigator))
            {
                this.Leader = instigator;
                this.EndGoal = endGoal;
                this.TargetFaction = target;
                if (initialGoal is null) { this.CurrentGoal = initialGoal; }
                else { this.CurrentGoal = this.EndGoal; }
                this.Uniqueto = uniqueto;
            }
            else throw new ArgumentException();

        }
        public override TriggerBase Trigger { get => new CivilWarTrigger(); }
        public override Uniqueto Uniqueto { get;  internal set; }
        public override string Name { get => throw new NotImplementedException(); internal set => throw new NotImplementedException(); }

    }
}
