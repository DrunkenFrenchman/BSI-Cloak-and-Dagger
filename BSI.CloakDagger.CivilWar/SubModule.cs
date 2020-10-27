using BSI.CloakDagger.Helpers;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using BSI.CloakDagger.CivilWar.CivilWar;
using BSI.CloakDagger.Objects;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Managers;

namespace BSI.CloakDagger.CivilWar
{
    public class SubModule : MBSubModuleBase
    {
        internal static Trigger Trigger { get; private set; }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (game.GameType is Campaign)
            {
                var campaignGameStarter = (CampaignGameStarter)gameStarter;

                try
                {
                    campaignGameStarter.AddBehavior(new CivilWarBehavior());

                    Trigger = new CivilWarTrigger();
                    Trigger.Initialize(UniqueTo.Kingdom, 1);
                    GameManager.Instance.AddTrigger(Trigger);
                }
                catch (Exception exception)
                {
                    Debug.AddExceptionLog("OnGameStart", exception);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Civil War", ColorHelper.Colors.Red));
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
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Civil War", ColorHelper.Colors.Green));
                }
                catch (Exception exception)
                {
                    Debug.AddExceptionLog("OnGameInitializationFinished", exception);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Civil War", ColorHelper.Colors.Red));
                }
            }
        }
    }
}