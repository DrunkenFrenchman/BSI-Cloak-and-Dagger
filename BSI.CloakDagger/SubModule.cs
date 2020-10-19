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
        private GameManager _gameManager { get; set; }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (game.GameType is Campaign)
            {
                var campaignGameStarter = (CampaignGameStarter)gameStarter;

                try
                {
                    _gameManager = new GameManager();
                    var (success, total) = _gameManager.InitializeTriggers();

                    campaignGameStarter.AddBehavior(_gameManager);

                    InformationManager.DisplayMessage(new InformationMessage($"Cloak and Dagger: Loaded {success} of {total} plots.", ColorHelper.Colors.Green));
                }
                catch (Exception ex)
                {
                    Debug.AddExceptionLog("OnGameStart", ex);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Failed to load plots!", ColorHelper.Colors.Red));
                }
            }
        }

        public override void OnGameInitializationFinished(Game game)
        {
            try
            {
                if (game.GameType is Campaign)
                {
                    _gameManager.InitializeGameObjects();
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Successfully initialized.", ColorHelper.Colors.Green));
                }
            }
            catch (Exception ex)
            {
                Debug.AddExceptionLog("OnGameInitializationFinished", ex);
                InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Failed to initialize!", ColorHelper.Colors.Red));
            }
        }
    }
}