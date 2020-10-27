using System;
using BSI.CloakDagger.CivilWar.CivilWar;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Managers;
using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BSI.CloakDagger.CivilWar
{
    public class SubModule : MBSubModuleBase
    {
        internal static Trigger Trigger { get; private set; }

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

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            if (!(game.GameType is Campaign))
            {
                return;
            }

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