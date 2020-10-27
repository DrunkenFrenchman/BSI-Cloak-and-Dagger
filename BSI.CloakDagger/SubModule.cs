using BSI.CloakDagger.Helpers;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using BSI.CloakDagger.Managers;

namespace BSI.CloakDagger
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (game.GameType is Campaign)
            {
                var campaignGameStarter = (CampaignGameStarter)gameStarter;

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
        }

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            if (game.GameType is Campaign)
            {
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
}