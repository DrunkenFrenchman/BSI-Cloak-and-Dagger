using BSI.CloakDagger.Models.PlotMod;
using Newtonsoft.Json;

namespace BSI.CloakDagger.Models
{
    internal class GamePlot
    {
        [JsonProperty]
        internal GameObject GameObject { get; set; }

        [JsonProperty]
        internal Plot Plot { get; set; }
    }
}