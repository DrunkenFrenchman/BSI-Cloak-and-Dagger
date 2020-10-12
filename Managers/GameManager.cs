using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace BSI.Manager
{
    [Serializable]
    sealed class GameManager : BaseManager<String, IBSIObjectBase>
    {
        public Game Game { get => this.Game; set => this.Game = value; }

        public GameManager(Game game)
        {
            this.Game = game;
            foreach (Kingdom kingdom in Kingdom.All)
            {
                this.Add(kingdom.StringId, new FactionInfo<Kingdom>(kingdom));
            }
        }

    }
}
