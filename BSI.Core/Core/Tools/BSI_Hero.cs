using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Tools
{
    public static class BSI_Hero
    {
        public static bool IsClanLeader(Hero hero)
        {
            return hero.Clan.Leader.Equals(hero);
        }

        public static Hero GetHero(string stringId, IFaction faction = null, FactionInfo info = null)
        {
            try
            {
                if (faction != null)
                {
                    foreach (Hero hero in faction.Heroes)
                    {
                        if (hero.StringId.Equals(stringId)) { return hero; }
                    }
                }
                if (info != null)
                {
                    foreach (Hero hero in info.Heroes)
                    {
                        if (hero.StringId.Equals(stringId)) { return hero; }
                    }
                }
                else
                {
                    foreach (Hero hero in Hero.All)
                    {
                        if (hero.StringId.Equals(stringId)) { return hero; }
                    }
                }
            }
            catch { throw new ArgumentException(); }
            return null;
        }
    }      
}
