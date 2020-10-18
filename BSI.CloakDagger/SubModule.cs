using BSI.Core.Helpers;
using BSI.Core;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BSI.CloakDagger
{
    public class SubModule : MBSubModuleBase
    {
        private GameManager _gameManager { get; set; }

        protected override void OnSubModuleLoad()
        {
            InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Loaded Engine!", ColorHelper.Colors.Green));
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (game.GameType is Campaign)
            {
                var campaignGameStarter = (CampaignGameStarter)gameStarter;

                try
                {
                    this._gameManager = new GameManager();
                    this._gameManager.InitializeTriggers();

                    campaignGameStarter.AddBehavior(this._gameManager);

                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Loaded Plots!", ColorHelper.Colors.Green));
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
                    this._gameManager.InitializeGameObjects();
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Initialized Engine!", ColorHelper.Colors.Green));
                }
            }
            catch (Exception ex)
            {
                Debug.AddExceptionLog("OnGameInitializationFinished", ex);
                InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Failed to initialize engine!", ColorHelper.Colors.Red));
            }
        }
    }
}