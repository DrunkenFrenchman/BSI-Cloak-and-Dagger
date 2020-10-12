using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace BSI.Manager
{
    [Serializable]
    sealed class GameManager : BaseManager<Game, IBSIObjectBase>
    {
        public Game Game { get => this.Game; set => this.Game = value; }

        public GameManager(Game game)
        {
            this.Game = game;
        }

    }
}
