using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public static class Tools
    {
        public static bool isClanLeader(Hero hero)
        {
            return hero.Clan.Leader.Equals(hero);
        }
    }
}
