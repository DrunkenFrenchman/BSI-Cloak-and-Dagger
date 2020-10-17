using BSI.Core.Helpers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BSI.Core.Managers
{
    public static class KingdomManager
    {
        public static Kingdom CreateKingdom(Hero leader, TextObject name, TextObject informalName, Banner banner = null, bool showNotification = false)
        {
            var kingdom = MBObjectManager.Instance.CreateObject<Kingdom>();
            Color MainColor = ColorGenerator.Random;
            Color AltColor = ColorGenerator.Random;
            kingdom.InitializeKingdom(name, informalName, leader.Culture, banner ?? Banner.CreateRandomClanBanner(leader.StringId.GetDeterministicHashCode()), MainColor.ToUnsignedInteger(), ColorGenerator.OpposingColor(MainColor).ToUnsignedInteger(), leader.Clan.InitialPosition);

            ChangeKingdomAction.ApplyByCreateKingdom(leader.Clan, kingdom, showNotification);

            AccessTools.Property(typeof(Kingdom), "AlternativeColor").SetValue(kingdom, AltColor.ToUnsignedInteger());
            AccessTools.Property(typeof(Kingdom), "AlternativeColor2").SetValue(kingdom, ColorGenerator.OpposingColor(MainColor).ToUnsignedInteger());

            return kingdom;
        }

        public static void ModifyKingdomList(Func<List<Kingdom>, List<Kingdom>> modificator)
        {
            var kingdoms = modificator(Campaign.Current.Kingdoms.ToList());
            if (kingdoms != null)
            {
                AccessTools.Field(Campaign.Current.GetType(), "_kingdoms").SetValue(Campaign.Current, new MBReadOnlyList<Kingdom>(kingdoms));
            }
        }

        public static void AddKingdom(Kingdom kingdom)
        {
            KingdomManager.ModifyKingdomList(kingdoms =>
            {
                if (kingdoms.Contains(kingdom))
                {
                    return null;
                }

                kingdoms.Add(kingdom);
                return kingdoms;
            });
        }

        public static void RemoveKingdom(Kingdom kingdom)
        {
            KingdomManager.ModifyKingdomList(kingdoms =>
            {
                return kingdoms.RemoveAll(k => k.StringId == kingdom.StringId) > 0 ? kingdoms : null;
            });
        }

        public static TextObject NameGenerator(Hero hero)
        {
            string name = " ";
            Enumerations.Culture culture = (Enumerations.Culture)hero.Culture.GetCultureCode();
            switch (culture)
            {
                case Enumerations.Culture.Aserai:
                    name += Enumerations.KingdomTitle.Sultanate.ToString();
                    break;
                case Enumerations.Culture.Battania:
                    name += Enumerations.KingdomTitle.Riocht.ToString();
                    break;
                case Enumerations.Culture.Empire:
                    name += Enumerations.KingdomTitle.Vasileo.ToString();
                    break;
                case Enumerations.Culture.Khuzait:
                    name += Enumerations.KingdomTitle.Khaganate.ToString();
                    break;
                case Enumerations.Culture.Sturgia:
                    name += Enumerations.KingdomTitle.Storveldi.ToString();
                    break;
                case Enumerations.Culture.Vlandia:
                    name += Enumerations.KingdomTitle.Royaume.ToString();
                    break;
            }

            return new TextObject(hero.Clan.Name.ToString() + name);

        }
    }
}