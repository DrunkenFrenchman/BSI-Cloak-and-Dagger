using System;
using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Managers;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BSI.CloakDagger
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            try
            {
                new Harmony(GetType().Namespace).PatchAll(GetType().Assembly);
            }
            catch (Exception exception)
            {
                Debug.AddExceptionLog("OnSubModuleLoad", exception);
                InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (!(game.GameType is Campaign))
            {
                return;
            }

            var campaignGameStarter = (CampaignGameStarter) gameStarter;

            try
            {
                campaignGameStarter.AddBehavior(GameManager.Instance);
            }
            catch (Exception exception)
            {
                Debug.AddExceptionLog("OnGameStart", exception);
                InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
            }
        }

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            if (!(game.GameType is Campaign))
            {
                return;
            }

            try
            {
                GameManager.Instance.Initialize();
                InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Green));
            }
            catch (Exception exception)
            {
                Debug.AddExceptionLog("OnGameInitializationFinished", exception);
                InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
            }
        }
    }
}