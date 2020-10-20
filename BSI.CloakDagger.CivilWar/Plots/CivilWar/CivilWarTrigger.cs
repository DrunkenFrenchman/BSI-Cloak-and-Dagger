using BSI.CloakDagger.CivilWar.Settings;
using BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.RecruitForWar;
using BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.WarForIndependence;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Extensions;
using BSI.CloakDagger.Objects;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar
{
    internal class CivilWarTrigger : Trigger
    {
        private static readonly CivilWarSettings settings = CivilWarSettings.Instance;

        internal CivilWarTrigger()
        {
            UniqueTo = UniqueTo.Kingdom;
            AllowedInstancesPerGameObject = 1;
        }

        public override bool CanStart(MBObjectBase gameObject)
        {
            if (!(gameObject is CharacterObject character) || character?.HeroObject == null)
            {
                return false;
            }

            var hero = character.HeroObject;

            var isValidHero = hero.Clan != null
                && hero.Clan.Kingdom != null
                && hero.Clan.Kingdom.Leader != hero
                && hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold
                && hero.Clan.Leader == hero
                && !hero.Clan.IsMinorFaction;
            if (!isValidHero)
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

            var plot = new CivilWarPlot();

            var goal = new RecruitForWarGoal(plot, target, SubModule.RecruitForWarBehavior);
            var endGoal = new WarForIndependenceGoal(plot, target, SubModule.WarForIndependenceBehavior);

            goal.NextPossibleGoals.Add(endGoal);
            
            plot.Initialize(target, leader, goal, endGoal);
            return plot;
        }
    }
}