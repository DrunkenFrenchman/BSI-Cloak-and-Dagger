using BSI.Core.Managers;
using BSI.Plots.CivilWar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BSI.Core
{
    class SubModule : MBSubModuleBase
    {
        public override void OnGameInitializationFinished(Game game)
        {
            if (game.GameType is Campaign)
            {
                Debug.AddEntry("New Game Started");
                GameManager.NewGame();
                Debug.AddEntry("Civil War Loaded");
            }
        }


        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        { 
            if (game.GameType is Campaign)
            {
                Debug.AddEntry("Adding Behavior");
                CampaignGameStarter cgs = gameStarterObject as CampaignGameStarter;
                cgs.AddBehavior(new GameManager.BSIConnector());
                Debug.AddEntry("Success!");
            }
        }

        protected override void OnSubModuleLoad()
        {
            Debug.AddEntry("Module Loaded");
        }

    }
}
