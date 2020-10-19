using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;

namespace BSI.Core
{
    public class MySettings : AttributeGlobalSettings<MySettings>
    {
        public override string Id => "BSI.Plots";
        public override string DisplayName => "BSI - Cloak and Dagger";
        public override string FolderName => "BSI.Plots";
        public override string Format => "json";

        //Main Settings for Wage Model
        [SettingPropertyBool("{=BSICORE_DEBUG}Debug", HintText = "{=BSICORE_DEBUG_DESC}Enables Debug Mode", Order = 0, RequireRestart = false)]
        public bool BSICORE_DEBUG { get; set; } = true;
    }
}