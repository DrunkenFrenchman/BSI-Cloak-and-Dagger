using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BSI.CivilWar.Core
{
    public abstract class Plot : IPlot
    {
        public IFaction ParentFaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsCivilWar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IBaseManager<string, Clan> Members { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IBaseManager<string, Clan> Opponents { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EndGoal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public TextObject Name => throw new NotImplementedException();

        public string StringId => throw new NotImplementedException();

        public TextObject InformalName => throw new NotImplementedException();

        public string EncyclopediaLink => throw new NotImplementedException();

        public string EncyclopediaLinkWithName => throw new NotImplementedException();

        public TextObject EncyclopediaText => throw new NotImplementedException();

        public CultureObject Culture => throw new NotImplementedException();

        public Vec2 InitialPosition => throw new NotImplementedException();

        public uint LabelColor => throw new NotImplementedException();

        public uint Color => throw new NotImplementedException();

        public uint Color2 => throw new NotImplementedException();

        public uint AlternativeColor => throw new NotImplementedException();

        public uint AlternativeColor2 => throw new NotImplementedException();

        public CharacterObject BasicTroop => throw new NotImplementedException();

        public Hero Leader => throw new NotImplementedException();

        public Banner Banner => throw new NotImplementedException();

        public IEnumerable<Settlement> Settlements => throw new NotImplementedException();

        public IEnumerable<Town> Fiefs => throw new NotImplementedException();

        public IEnumerable<Hero> Lords => throw new NotImplementedException();

        public IEnumerable<Hero> Heroes => throw new NotImplementedException();

        public IEnumerable<MobileParty> AllParties => throw new NotImplementedException();

        public IEnumerable<MobileParty> WarParties => throw new NotImplementedException();

        public bool IsBanditFaction => throw new NotImplementedException();

        public bool IsMinorFaction => throw new NotImplementedException();

        public bool IsKingdomFaction => throw new NotImplementedException();

        public bool IsClan => throw new NotImplementedException();

        public bool IsOutlaw => throw new NotImplementedException();

        public bool IsMapFaction => throw new NotImplementedException();

        public IFaction MapFaction => throw new NotImplementedException();

        public float TotalStrength => throw new NotImplementedException();

        public Vec2 FactionMidPoint => throw new NotImplementedException();

        public IEnumerable<StanceLink> Stances => throw new NotImplementedException();

        public int TributeWallet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float MainHeroCrimeRating { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float DailyCrimeRatingChange => throw new NotImplementedException();

        public float Aggressiveness => throw new NotImplementedException();

        public bool IsEliminated => throw new NotImplementedException();

        public StatExplainer DailyCrimeRatingChangeExplained => throw new NotImplementedException();

        public CampaignTime NotAttackableByPlayerUntilTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public StanceLink GetStanceWith(IFaction other)
        {
            throw new NotImplementedException();
        }

        public bool IsAtWarWith(IFaction other)
        {
            throw new NotImplementedException();
        }
    }
}
