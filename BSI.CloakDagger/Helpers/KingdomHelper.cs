using System;
using System.Collections.Generic;
using System.Linq;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Extensions;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Helpers
{
    public static class KingdomHelper
    {
        public static Kingdom CreateKingdom(Hero leader, TextObject name, TextObject informalName, Banner banner = null, bool showNotification = false)
        {
            var mainColor = ColorHelper.GetRandomColor();
            var altColor = ColorHelper.GetRandomColor();

            var kingdom = MBObjectManager.Instance.CreateObject<Kingdom>();
            kingdom.InitializeKingdom(name, informalName, leader.Culture, banner ?? Banner.CreateRandomClanBanner(leader.StringId.GetDeterministicHashCode()), mainColor.ToUnsignedInteger(), mainColor.GetOpposingColor().ToUnsignedInteger(), leader.Clan.InitialPosition);

            ChangeKingdomAction.ApplyByCreateKingdom(leader.Clan, kingdom, showNotification);

            AccessTools.Property(typeof(Kingdom), "AlternativeColor").SetValue(kingdom, altColor.ToUnsignedInteger());
            AccessTools.Property(typeof(Kingdom), "AlternativeColor2").SetValue(kingdom, mainColor.GetOpposingColor().ToUnsignedInteger());

            AddKingdom(kingdom);
            return kingdom;
        }

        public static TextObject GenerateName(Hero hero)
        {
            var name = " ";
            var culture = (Culture) hero.Culture.GetCultureCode();
            switch (culture)
            {
                case Culture.Aserai:
                    name += KingdomTitle.Sultanate.ToString();
                    break;
                case Culture.Battania:
                    name += KingdomTitle.Riocht.ToString();
                    break;
                case Culture.Empire:
                    name += KingdomTitle.Vasileo.ToString();
                    break;
                case Culture.Khuzait:
                    name += KingdomTitle.Khaganate.ToString();
                    break;
                case Culture.Sturgia:
                    name += KingdomTitle.Storveldi.ToString();
                    break;
                case Culture.Vlandia:
                    name += KingdomTitle.Royaume.ToString();
                    break;
            }

            return new TextObject(hero.Clan.Name + name);
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
            ModifyKingdomList(kingdoms =>
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
            ModifyKingdomList(kingdoms =>
            {
                return kingdoms.RemoveAll(k => k.StringId == kingdom.StringId) > 0 ? kingdoms : null;
            });
        }
    }
}