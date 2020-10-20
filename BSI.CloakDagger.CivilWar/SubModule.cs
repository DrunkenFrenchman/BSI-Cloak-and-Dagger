using BSI.CloakDagger.Helpers;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.WarForIndependence;
using BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.RecruitForWar;
using BSI.CloakDagger.CivilWar.Plots.CivilWar;

namespace BSI.CloakDagger.CivilWar
{
    public class SubModule : MBSubModuleBase
    {
        internal static RecruitForWarBehavior RecruitForWarBehavior { get; private set; }

        internal static WarForIndependenceBehavior WarForIndependenceBehavior { get; private set; }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (game.GameType is Campaign)
            {
                var campaignGameStarter = (CampaignGameStarter)gameStarter;

                try
                {
                    campaignGameStarter.AddBehavior(new CivilWarBehavior());
                    campaignGameStarter.AddBehavior(RecruitForWarBehavior = new RecruitForWarBehavior());
                    campaignGameStarter.AddBehavior(WarForIndependenceBehavior = new WarForIndependenceBehavior());

                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Civil War", ColorHelper.Colors.Green));
                }
                catch (Exception ex)
                {
                    Debug.AddExceptionLog("OnGameStart", ex);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Civil War", ColorHelper.Colors.Red));
                }
            }
        }
    }
}