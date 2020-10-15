using BSI.Core;
using BSI.Core.Flags;
using BSI.Core.Managers;
using BSI.Core.Objects;
using BSI.Core.Tools;
using System;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;

namespace BSI.Plots.CivilWar
{
    public class CivilWar : Plot
    {
        public CivilWar(Hero instigator, AvailableGoals endGoal, IFaction target, AvailableGoals initialGoal = 0, Uniqueto uniqueto = Uniqueto.Kingdom) : base(instigator, endGoal, target, initialGoal)
        {

            if (instigator.Clan.Leader.Equals(instigator) && !instigator.Clan.Kingdom.Leader.Equals(instigator))
            {
                this.Leader = instigator;
                this.EndGoal = endGoal;
                this.TargetFaction = target;
                this.EndGoal = endGoal;

                new PlotTools().GetGoal(endGoal, target, out Goal newGoal);
                this.CurrentGoal = newGoal;

                this.Uniqueto = uniqueto;
            }
            else throw new ArgumentException();

        }
        public override TriggerBase Trigger { get => new CivilWarTrigger(); }

    }
}
