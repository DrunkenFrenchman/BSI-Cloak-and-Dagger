using BSI.CloakDagger.Settings.Localization;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;

namespace BSI.CloakDager.Settings
{
    public class CloakDaggerSettings : AttributeGlobalSettings<CloakDaggerSettings>
    {
        public override string Id => $"BSI.CloakDagger_v{typeof(CloakDaggerSettings).Assembly.GetName().Version.ToString(3)}";

        public override string DisplayName => $"BSI - Cloak and Dagger";

        public override string FolderName => "BSI.CloakDagger";

        public override string FormatType => "json2";

        #region DEBUG

        [SettingPropertyGroup(groupName: Categories.DEBUG, GroupOrder = 1)]
        [SettingPropertyBool(displayName: Categories.DEBUG, Order = 0, HintText = Descriptions.DEBUG, RequireRestart = false)]
        public bool EnableDebug { get; set; } = true;

        #endregion
    }
}