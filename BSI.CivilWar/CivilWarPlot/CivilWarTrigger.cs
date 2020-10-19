﻿using BSI.CivilWar.Goals;
using BSI.CivilWar.Goals.WarForIndependence;
using BSI.Core.Enumerations;
using BSI.Core.Extensions;
using BSI.Core.Objects;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CivilWar
{
    public class CivilWarTrigger : Trigger
    {
        private static readonly MySettings settings = MySettings.Instance;

        public CivilWarTrigger()
        {
            base.UniqueTo = UniqueTo.Kingdom;
            base.AllowedInstancesPerGameObject = 1;
        }

        public override bool CanStart(MBObjectBase gameObject)
        {
            if(!(gameObject is Hero hero))
            {
                return false;
            }

            var isValidHero = hero.Clan != null
                && hero.Clan.Kingdom != null
                && hero.Clan.Kingdom.Leader != hero
                && hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold
                && hero.Clan.Leader == hero
                && !hero.Clan.IsMinorFaction;
            if(!isValidHero)
            {
                return false;
            }

            var tick = new Random().Next(100);
            var honorScore = -(hero.GetCharacterTraitLevel(CharacterTrait.Honor) + hero.Clan.Kingdom.Leader.GetCharacterTraitLevel(CharacterTrait.Honor));
            var plottingChance = 10 * Math.Pow(2f, honorScore);
            return plottingChance > tick;
        }

        public override Plot Start(MBObjectBase gameObject)
        {
            var target = gameObject.ConvertToKingdom();
            var leader = gameObject.ConvertToHero();

            var goal = new RecruitForWarGoal(target, new RecruitForWarBehavior());
            var endGoal = new WarForIndependenceGoal(target, new WarForIndependenceBehavior());

            goal.NextPossibleGoals.Add(endGoal);

            return new CivilWarPlot(target, leader, goal, endGoal, UniqueTo.Kingdom);
        }
    }
}