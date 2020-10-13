using BSI.Core.Managers;
using BSI.Manager;
using BSI.Plots.CivilWar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BSI.Core
{
    class SubModule : MBSubModuleBase
    {


        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);
        }


        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            Debug.AddEntry("New Game Started");
            GameManager.NewGame(game);
            Debug.AddEntry("New Game Setup Finished");
        }

        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            Debug.AddEntry("New Game Started");
            GameManager.NewGame(game);
            Debug.AddEntry("New Game Setup Finished");
        }

        protected override void OnSubModuleLoad()
        {
            Debug.AddEntry("Module Loaded");
            APIManager.LoadPlot(new CivilWar());
            Debug.AddEntry("Civil War Loaded");
        }

    }
}
