using BSI.CloakDagger.Helpers;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.WarForIndependence;
using BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.RecruitForWar;

namespace BSI.CloakDagger.CivilWar
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger CivilWar: Loaded Mod!", ColorHelper.Colors.Green));
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (game.GameType is Campaign)
            {
                var campaignGameStarter = (CampaignGameStarter)gameStarter;

                try
                {
                    campaignGameStarter.AddBehavior(new RecruitForWarBehavior());
                    campaignGameStarter.AddBehavior(new WarForIndependenceBehavior());

                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Loaded CivilWar!", ColorHelper.Colors.Green));
                }
                catch (Exception ex)
                {
                    Debug.AddExceptionLog("OnGameStart", ex);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Failed to load CivilWar!", ColorHelper.Colors.Red));
                }
            }
        }

        public override void OnGameInitializationFinished(Game game)
        {
            try
            {
                if (game.GameType is Campaign)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Initialized CivilWar!", ColorHelper.Colors.Green));
                }
            }
            catch (Exception ex)
            {
                Debug.AddExceptionLog("OnGameInitializationFinished", ex);
                InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Failed to initialize CivilWar!", ColorHelper.Colors.Red));
            }
        }
    }
}