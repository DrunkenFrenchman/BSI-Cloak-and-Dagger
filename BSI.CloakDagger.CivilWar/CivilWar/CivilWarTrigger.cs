using System;
using System.Collections.Generic;
using System.Linq;
using BSI.CloakDagger.CivilWar.CivilWar.Goals.RecruitForWar;
using BSI.CloakDagger.CivilWar.CivilWar.Goals.WarForIndependence;
using BSI.CloakDagger.CivilWar.Settings;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Extensions;
using BSI.CloakDagger.Managers;
using BSI.CloakDagger.Objects;

namespace BSI.CloakDagger.CivilWar.CivilWar
{
    public class CivilWarTrigger : Trigger
    {
        private static readonly CivilWarSettings Settings = CivilWarSettings.Instance;

        public override bool CanStart(GameObject gameObject)
        {
            var hero = gameObject.ToHero();
            if (hero.Clan?.Kingdom == null || hero.Clan.Kingdom.Leader == hero || !hero.IsClanLeader() || hero.GetRelation(hero.Clan.Kingdom.Leader) > Settings.NegativeRelationThreshold || hero.Clan.IsMinorFaction)
            {
                return false;
            }

            if (GameManager.Instance.PlotManager.GetPlots(nameof(CivilWarTrigger)).FirstOrDefault(p => p.Leader == gameObject || p.Members.Contains(gameObject)) != null)
            {
                return false;
            }

            var honorScore = -(hero.GetCharacterTraitLevel(CharacterTrait.Honor) + hero.Clan.Kingdom.Leader.GetCharacterTraitLevel(CharacterTrait.Honor));
            var plottingChance = 10 * Math.Pow(2f, honorScore);
            return plottingChance > new Random().Next(100);
        }

        public override Plot DoStart(GameObject gameObject)
        {
            var target = gameObject.ToKingdom();
            var leader = gameObject.ToHero();
            var members = new List<GameObject>
            {
                gameObject
            };

            var plot = new CivilWarPlot();
            var plotTitle = "Civil War";
            var plotDescription = $"This plot aims to reach independence from {target.Name} through Civil War.";

            var recruitForWarGoal = new RecruitForWarGoal();
            var recruitForWarGoalTitle = "Recruit for war";
            var recruitForWarGoalDescription = $"Recruit plot members for war against {target.Name}.";
            recruitForWarGoal.Initialize(recruitForWarGoalTitle, recruitForWarGoalDescription, null, plot);

            var warForIndependenceGoal = new WarForIndependenceGoal();
            var warForIndependenceGoalTitle = "War for independence";
            var warForIndependenceGoalDescription = $"Do a civil war for independence against {target.Name}.";
            warForIndependenceGoal.Initialize(warForIndependenceGoalTitle, warForIndependenceGoalDescription, null, plot);

            plot.Initialize(plotTitle, plotDescription, target.ToGameObject(), leader.ToGameObject(), members, recruitForWarGoal, warForIndependenceGoal, nameof(CivilWarTrigger));

            return plot;
        }
    }
}