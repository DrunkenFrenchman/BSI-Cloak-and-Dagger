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
            var hero = gameObject.ConvertToHero();
            if (hero == null)
            {
                return false;
            }

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

            var goal = new RecruitForWarGoal(target, new RecruitForWarBehavior());
            var endGoal = new WarForIndependenceGoal(target, new WarForIndependenceBehavior());

            goal.NextPossibleGoals.Add(endGoal);

            return new CivilWarPlot(target, leader, goal, endGoal);
        }
    }
}