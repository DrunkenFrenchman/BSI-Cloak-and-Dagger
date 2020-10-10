using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;



namespace BSI.CivilWar
{
    public class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => "BSICivilWars";
        public override string DisplayName => "BSI Civil Wars";
        public override string FolderName => "BSICivilWars";
        public override string Format => "json";

        //Main Settings for Wage Model
        //[SettingPropertyBool("{=BSIWM_SETTING_08}1. Wage Model", HintText = "{=BSIWM_SETTING_DESC_12}Check this to enable 'Wage Model'", Order = 0, RequireRestart = true)]
        //[SettingPropertyGroup("{=BSIWM_SETTING_GROUP_03}1. Wage Model", GroupOrder = 0, IsMainToggle = true)]
        //public bool BSIWageModelToggle { get; set; } = true;
        //[SettingPropertyInteger("{=BSIWM_SETTING_32}Minimum Troop Wage", 1, 25, HintText = "{=BSIWM_SETTING_DESC_32}This number determines the minimum wage paid to a troop. MUST BE SMALLER THAN MAX WAGE", Order = 1, RequireRestart = false)]
        //[SettingPropertyGroup("{=BSIWM_SETTING_GROUP_01}1. Wage Model", GroupOrder = 0)]
        //public int BSIMinWage { get; set; } = 1;

        //Debugger Toggle
        [SettingPropertyBool("{=BSIWM_SETTING_DEBUG}Debugging", HintText = "{=BSIWM_SETTING_DESC_DEBUG}Check this to enable Debug mode", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_03}4. Debug", GroupOrder = 3)]
        public bool BSICWDebug { get; set; } = true;



    }
}