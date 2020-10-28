using System;
using System.Collections.Generic;
using BSI.CloakDagger.CivilWar.CivilWar.Goals.RecruitForWar;
using BSI.CloakDagger.CivilWar.CivilWar.Goals.WarForIndependence;
using BSI.CloakDagger.CivilWar.Settings;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Extensions;
using BSI.CloakDagger.Objects;

namespace BSI.CloakDagger.CivilWar.CivilWar
{
    public class CivilWarTrigger : Trigger
    {
        private static readonly CivilWarSettings Settings = CivilWarSettings.Instance;

        public override bool CanStart(GameObject gameObject)
        {
            if (gameObject.GameObjectType != GameObjectType.Hero)
            {
                return false;
            }

            var hero = gameObject.ToHero();

            if (hero.Clan?.Kingdom == null || hero.Clan.Kingdom.Leader == hero || !hero.IsClanLeader() || hero.GetRelation(hero.Clan.Kingdom.Leader) > Settings.NegativeRelationThreshold || hero.Clan.IsMinorFaction)
            {
                return false;
            }

            var tick = new Random().Next(100);
            var honorScore = -(hero.GetCharacterTraitLevel(CharacterTrait.Honor) + hero.Clan.Kingdom.Leader.GetCharacterTraitLevel(CharacterTrait.Honor));
            var plottingChance = 10 * Math.Pow(2f, honorScore);
            return plottingChance > tick;
        }

        public override Plot DoStart(GameObject gameObject)
        {
            var target = gameObject.ToKingdom();
            var leader = gameObject.ToHero();
            var members = new List<GameObject>
            {
                gameObject
            };

            var civilWarPlot = new CivilWarPlot();
            var civilWarPlotTitle = "Civil War";
            var civilWarPlotDescription = $"This plot aims to reach independence from {target.Name} through Civil War.";

            var recruitForWarGoal = new RecruitForWarGoal();
            var recruitForWarGoalTitle = "Recruit for war";
            var recruitForWarGoalDescription = $"Recruit plot members for war against {target.Name}.";
            recruitForWarGoal.Initialize(recruitForWarGoalTitle, recruitForWarGoalDescription, null, civilWarPlot);

            var warForIndependenceGoal = new WarForIndependenceGoal();
            var warForIndependenceGoalTitle = "War for independence";
            var warForIndependenceGoalDescription = $"Do a civil war for independence against {target.Name}.";
            warForIndependenceGoal.Initialize(warForIndependenceGoalTitle, warForIndependenceGoalDescription, null, civilWarPlot);

            civilWarPlot.Initialize(civilWarPlotTitle, civilWarPlotDescription, target.ToGameObject(), leader.ToGameObject(), members, recruitForWarGoal, warForIndependenceGoal, GetType().Name);

            return civilWarPlot;
        }
    }
}