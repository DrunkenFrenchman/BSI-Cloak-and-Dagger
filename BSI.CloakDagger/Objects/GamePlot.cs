using Newtonsoft.Json;

namespace BSI.CloakDagger.Objects
{
    internal class GamePlot
    {
        [JsonProperty] internal GameObject GameObject { get; set; }

        [JsonProperty] internal Plot Plot { get; set; }
    }
}