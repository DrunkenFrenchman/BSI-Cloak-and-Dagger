using BSI.CivilWar.Goals;
using BSI.CivilWar.Goals.WarForIndependence;
using BSI.Core;
using BSI.Core.Enumerations;
using BSI.Core.Extensions;
using BSI.Core.Objects;
using System;
using System.Collections.Generic;
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
            var hero = HeroExtension.ConvertToHero(gameObject);
            
            var goal = new RecruitForWarGoal((Kingdom)gameObject, new RecruitForWarBehavior());
            var endGoal = new WarForIndependenceGoal(HeroExtension.ConvertToKingdom(gameObject), new WarForIndependenceBehavior());
            var nextGoals = new List<Goal>();
            nextGoals.Add(endGoal);
            return new CivilWarPlot(hero.Clan.Kingdom, hero, goal, endGoal, UniqueTo.NotSet);
        }
    }
}