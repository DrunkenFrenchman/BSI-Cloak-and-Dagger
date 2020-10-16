using BSI.Core.Flags;
using BSI.Core.Managers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BSI.Core.Tools
{
    class KingdomTools
    {
        //Credit to TH3UNKNOWN for this method
        public static Kingdom CreateKingdom(Hero leader, TextObject name, TextObject informalName, Banner banner = null, bool showNotification = false)
        {
            var kingdom = MBObjectManager.Instance.CreateObject<Kingdom>();
            kingdom.InitializeKingdom(name, informalName, leader.Culture, banner ?? Banner.CreateRandomClanBanner(leader.StringId.GetDeterministicHashCode()), leader.Clan.Color, leader.Clan.Color2, leader.Clan.InitialPosition);
            GameManager.EventManager.AddFaction(kingdom, true);
            ChangeKingdomAction.ApplyByJoinToKingdom(leader.Clan, kingdom, showNotification);
            kingdom.RulingClan = leader.Clan;

            AccessTools.Property(typeof(Kingdom), "AlternativeColor").SetValue(kingdom, leader.Clan.Color);
            AccessTools.Property(typeof(Kingdom), "AlternativeColor2").SetValue(kingdom, leader.Clan.Color2);

            GameManager.EventManager.AddFaction(kingdom);
            return kingdom;
        }

        public static void UpdateKingdomList()
        {
            List<Kingdom> kingdoms = new List<Kingdom>();
            
            foreach (IFaction faction in GameManager.FactionManager.Keys) { if (faction.GetType() == typeof(Kingdom)) { kingdoms.Add((Kingdom)faction); } }

            AccessTools.Field(Campaign.Current.GetType(), "_kingdoms").SetValue(Campaign.Current, new MBReadOnlyList<Kingdom>(kingdoms));
        }

        public static TextObject NameGenerator(Hero hero)
        {
            string name = " ";
            Flags.Cultures culture = (Flags.Cultures) hero.Culture.GetCultureCode();
            switch (culture)
            {
                case Flags.Cultures.Aserai:
                    name += KingdomTitles.Sultanate.ToString();
                    break;
                case Flags.Cultures.Battania:
                    name += KingdomTitles.Riocht.ToString();
                    break;
                case Flags.Cultures.Empire:
                    name += KingdomTitles.Vasileo.ToString();
                    break;
                case Flags.Cultures.Khuzait:
                    name += KingdomTitles.Khaganate.ToString();
                    break;
                case Flags.Cultures.Sturgia:
                    name += KingdomTitles.Storveldi.ToString();
                    break;
                case Flags.Cultures.Vlandia:
                    name += KingdomTitles.Royaume.ToString();
                    break;
            }

            return  new TextObject(hero.Clan.Name.ToString() + name);

        }
    }
}
