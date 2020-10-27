using BSI.CloakDagger.Settings.Localization;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;

namespace BSI.CloakDagger.Settings
{
    public class CloakDaggerSettings : AttributeGlobalSettings<CloakDaggerSettings>
    {
        public override string Id => $"BSI.CloakDagger_v{typeof(CloakDaggerSettings).Assembly.GetName().Version.ToString(3)}";

        public override string DisplayName => "BSI - Cloak and Dagger";

        public override string FolderName => "BSI.CloakDagger";

        public override string FormatType => "json2";

        #region DEBUG

        [SettingPropertyGroup(Categories.Debug, GroupOrder = 1)]
        [SettingPropertyBool(Categories.Debug, Order = 0, HintText = Descriptions.Debug, RequireRestart = false)]
        public bool EnableDebug { get; set; } = true;

        #endregion
    }
}