using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;
using BSI.Core.Tools;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace BSI.Manager
{
    [Serializable]
    public static class GameManager
    {
        public static Game CurrentGame;

        public static readonly List<IBSIObjectBase> Kingdoms = new List<IBSIObjectBase>();

        public static readonly PlotManager GlobalPlots = new PlotManager(CurrentGame);

        public static void NewaGme(Game game)
        {
            GameManager.CurrentGame = game;
            GameManager.UpdateKingdoms();
        }
        public static void UpdateKingdoms()
        {
            if (!GameManager.Kingdoms.IsEmpty())
            {
                GameManager.Kingdoms.Clear();
            }
            
            Debug.AddEntry(Kingdom.All.ToString());

            foreach (Kingdom kingdom in Kingdom.All)
            {

                PlotManager temp;
                if ( BSI_Faction.GetKingdom(kingdom) is null ) { temp = BSI_Faction.GetKingdom(kingdom).PlotManager; }
                else { temp = new PlotManager(); }
                
                GameManager.Kingdoms.Add(new FactionInfo(kingdom, temp));
            }
        }

    }
}
