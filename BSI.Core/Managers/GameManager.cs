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
    public sealed class GameManager : BaseManager<IBSIObjectBase>, IManager<IBSIObjectBase>, IBSIManagerBase
    {
        public static Game CurrentGame;

        public static readonly List<IBSIObjectBase> Kingdoms;

        public static readonly PlotManager GlobalPlots = new PlotManager(CurrentGame);

        public static void NewGame(Game game)
        {
            GameManager.CurrentGame = game;
            GameManager.UpdateKingdoms();
        }

        public static void UpdateKingdoms()
        {
            GameManager.Kingdoms.Clear();

            foreach (Kingdom kingdom in Kingdom.All)
            {
                GameManager.Kingdoms.Add(new FactionInfo(kingdom));
            }
        }
    }
}
