using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;
using TaleWorlds.Localization;

namespace BSICivilWars.Helpers
{
    public interface IFactionInfo<TypeInfo> where TypeInfo : IFaction
    {
        public string StringId => StringId;
    }
}
