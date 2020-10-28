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
                LogHelper.LogException("OnSubModuleLoad", exception);
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

            try
            {
                ((CampaignGameStarter) gameStarter).AddBehavior(GameManager.Instance);
            }
            catch (Exception exception)
            {
                LogHelper.LogException("OnGameStart", exception);
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
                SaveFileManager.Instance.LoadData();
                InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Green));
            }
            catch (Exception exception)
            {
                LogHelper.LogException("OnGameInitializationFinished", exception);
                InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
            }
        }
    }
}