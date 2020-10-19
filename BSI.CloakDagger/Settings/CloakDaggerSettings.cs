﻿using BSI.CloakDagger.Settings.Localization;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;

namespace BSI.CloakDagger
{
    public class CloakDaggerSettings : AttributeGlobalSettings<CloakDaggerSettings>
    {
        public override string Id => $"BSI.CloakDagger_v{typeof(CloakDaggerSettings).Assembly.GetName().Version.ToString(3)}";

        public override string DisplayName => $"BSI - Cloak and Dagger {typeof(CloakDaggerSettings).Assembly.GetName().Version.ToString(3)}";

        public override string FolderName => "BSI.CloakDagger";

        public override string Format => "json";

        #region DEBUG

        [SettingPropertyGroup(groupName: Categories.DEBUG, GroupOrder = 1, IsMainToggle = true)]
        [SettingPropertyBool(displayName: Categories.DEBUG, Order = 0, HintText = Descriptions.DEBUG, RequireRestart = false)]
        public bool EnableMiscellaneous { get; set; } = true;

        #endregion
    }
}