﻿using BSI.Core;
using BSI.Core.Objects;
using BSI.Core.Tools;
using System;
using TaleWorlds.CampaignSystem;

namespace BSI.Plots.CivilWar
{
    public class CivilWar : Plot
    { 
        private static readonly BSI.Core.MySettings settings = BSI.Core.MySettings.Instance;
        
        private static readonly TriggerBase CivilWarTrigger = new CivilWarTrigger();

        public CivilWar(Hero instigator, Goal endGoal, Goal initialGoal = null, Uniqueto uniqueto = Uniqueto.Kingdom) : base(instigator, endGoal, initialGoal)
        {

            if (instigator.Clan.Leader.Equals(instigator) && !instigator.Clan.Kingdom.Leader.Equals(instigator))
            {
                this.Leader = instigator;
                this.EndGoal = endGoal;
                if (initialGoal is null) { this.CurrentGoal = initialGoal; }
                else { this.CurrentGoal = this.EndGoal; }
                this.Uniqueto = uniqueto;
            }
            else throw new ArgumentException();

        }

        public override Uniqueto Uniqueto { get;  internal set; }
        public override string Name { get => throw new NotImplementedException(); internal set => throw new NotImplementedException(); }
        public override TriggerBase Trigger { get => CivilWar.CivilWarTrigger; }
    }
}