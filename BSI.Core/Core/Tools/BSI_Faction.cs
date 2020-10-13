using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Tools
{
    public static class BSI_Faction
    {
        public static readonly Dictionary<string, FactionInfo> Lookup = new Dictionary<string, FactionInfo>();

        public static FactionInfo GetKingdom(Kingdom kingdom)
        {
            Lookup.TryGetValue(kingdom.StringId, out FactionInfo factioninfo);
            return factioninfo;
        }
    }
}
