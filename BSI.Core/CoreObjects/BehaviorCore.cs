﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.BannerEditor;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public abstract class BehaviorCore
    {
        internal abstract bool CanPlot(Hero hero);
        public abstract bool WantPlot(Hero hero);
        public abstract bool DoPlot(Hero hero);
        public abstract bool IsNewLeader(Hero hero);
        public abstract bool LeaveCondition(Hero hero);
        public abstract bool EndCondition();
        public abstract bool EndResult();
        public virtual bool OnDailyTick()
        {
            throw new NotImplementedException();
        }
        public virtual bool OnGameLoad()
        {
            throw new NotImplementedException();
        }
        public virtual void BeginGameStart(Game game)
        {
            throw new NotImplementedException();
        }

        public virtual bool DoLoading(Game game)
        {
            throw new NotImplementedException();
        }

        public virtual void OnCampaignStart(Game game, object starterObject)
        {
            throw new NotImplementedException();
        }

        public virtual void OnGameEnd(Game game)
        {
            throw new NotImplementedException();
        }

        public virtual void OnGameInitializationFinished(Game game)
        {
            throw new NotImplementedException();
        }

        public virtual void OnGameLoaded(Game game, object initializerObject)
        {
            throw new NotImplementedException();
        }

        public virtual void OnMissionBehaviourInitialize(Mission mission)
        {
            throw new NotImplementedException();
        }

        public virtual void OnMultiplayerGameStart(Game game, object starterObject)
        {
            throw new NotImplementedException();
        }

        public virtual void OnNewGameCreated(Game game, object initializerObject)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnApplicationTick(float dt)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnBeforeInitialModuleScreenSetAsRoot()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnSubModuleLoad()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnSubModuleUnloaded()
        {
            throw new NotImplementedException();
        }

    }
}
