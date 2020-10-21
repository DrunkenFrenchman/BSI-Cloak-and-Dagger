using BSI.CloakDagger.Helpers;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using BSI.CloakDagger.CivilWar.Plots.CivilWar;
using BSI.CloakDagger.Managers;

namespace BSI.CloakDagger.CivilWar
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (game.GameType is Campaign)
            {
                try
                {
                    GameManager.Instance.AddTrigger(new CivilWarTrigger());
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