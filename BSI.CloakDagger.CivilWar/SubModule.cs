using System;
using BSI.CloakDagger.CivilWar.CivilWar;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Managers;
using BSI.CloakDagger.Models.PlotMod;
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
                Trigger.Initialize(GameObjectType.Kingdom, "Civil War", "This plot aims to reach independence from the current kingdom through Civil War.", GameObjectType.Hero, 1);
                GameManager.Instance.AddTrigger(Trigger);
            }
            catch (Exception exception)
            {
                LogHelper.LogException("OnGameStart", exception);
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
                LogHelper.LogException("OnGameInitializationFinished", exception);
                InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger: Civil War", ColorHelper.Colors.Red));
            }
        }
    }
}