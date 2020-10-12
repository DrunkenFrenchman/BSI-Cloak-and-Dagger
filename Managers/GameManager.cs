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
    sealed class GameManager : BaseManager<IBSIObjectBase>
    {
        public Game Game { get => this.Game; set => this.Game = value; }

        public GameManager(Game game)
        {
            this.Game = game;
            this.Update();
        }

        public void Update()
        {
            GameManager temp = new GameManager(Game);

            foreach (Kingdom kingdom in Kingdom.All)
            {
                temp.Add(new FactionInfo<Kingdom>(kingdom));
                foreach (PlotManager plot in this)
                {
                    temp.Add(plot);
                }
            }
            this.Clear();
            this.AddRange(temp);
        }
    }
}
