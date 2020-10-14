
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;

namespace BSI.Core
{
    public class MySettings : AttributeGlobalSettings<MySettings>
    {
        public override string Id => "BSI.Plots";
        public override string DisplayName => "BSI - Cloak and Dagger";
        public override string FolderName => "BSI.Plots";
        public override string Format => "json";

        //Main Settings for Wage Model
        [SettingPropertyBool("{=BSIPLOTS_SETTING_01}1. Civil War", HintText = "{=BSIPLOTS_SETTING_DESC_01}Check this to enable Civil Wars", Order = 0, RequireRestart = true)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War", GroupOrder = 0, IsMainToggle = true)]
        public bool CivilWarToggle { get; set; } = true;

        [SettingPropertyInteger("{=BSIPLOTS_SETTING_02}1. Civil War", 1, 100, HintText = "{=BSIPLOTS_SETTING_DESC_02}Base Chance for a Lord to plot against his Liege if he meets the requirements", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/1.Plotting Phase", GroupOrder = 0)]
        public int BasePlotChance{ get; set; } = 10;

        [SettingPropertyInteger("{=BSIPLOTS_SETTING_03}1. Civil War", -100, 100, HintText = "{=BSIPLOTS_SETTING_DESC_03}Threshold at which a Lord can start Plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/1.Plotting Phase", GroupOrder = 0)]
        public int NegativeRelationThreshold { get; set; } = -10;

        [SettingPropertyInteger("{=BSIPLOTS_SETTING_04}1. Civil War", -100, 100, HintText = "{=BSIPLOTS_SETTING_DESC_04}Threshold at which a Lord will stop Plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/1.Plotting Phase", GroupOrder = 0)]
        public int PositiveRelationThreshold { get; set; } = 10;

        [SettingPropertyFloatingInteger("{=BSIPLOTS_SETTING_05}1. Civil War", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_05}Threshold at which a Lord will stop Plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/1.Plotting Phase", GroupOrder = 0)]
        public float PlotPersonalityMult { get; set; } = 2f;

        [SettingPropertyFloatingInteger("{=BSIPLOTS_SETTING_06}1. Civil War", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_06}Threshold at which a Lord will stop Plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/1.Plotting Phase", GroupOrder = 0)]
        public float PlotFriendMult { get; set; } = 2f;

        [SettingPropertyInteger("{=BSIPLOTS_SETTING_07}1. Civil War", 0, 100, HintText = "{=BSIPLOTS_SETTING_DESC_07}Threshold at which a Lord will stop Plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float WarBaseChance { get; set; } = 5;

        [SettingPropertyFloatingInteger("{=BSIPLOTS_SETTING_08}1. Civil War", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_08}Threshold at which a Lord will stop Plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float WarPersonalityMult { get; set; } = 2f;

        [SettingPropertyFloatingInteger("{=BSIPLOTS_SETTING_09}1. Civil War", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_09}Threshold at which a Lord will stop Plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float WarValorFactor { get; set; } = 2f;

        [SettingPropertyFloatingInteger("{==BSIPLOTS_SETTING_10}1. Civil War", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_10}Threshold at which a Lord will stop Plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float WarLeaderCalculatingMult { get; set; } = 2f;

        //Debug Toggle
        [SettingPropertyBool("{=BSIWM_SETTING_DEBUG}Debug Toggle", HintText = "{=BSIWM_SETTING_DESC_DEBUG}Check this to enable Debug mode", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_02}2. Debug", GroupOrder = 3)]
        public bool BSIPlotsDebug { get; set; } = true;

        public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets()
        {
            var basePresets = base.GetAvailablePresets();
            basePresets.Add("Realistic Battle Mod", () => new MySettings()
            {
               
            }); ;
            basePresets.Add("Native", () => new MySettings()
            {
          
            });
            basePresets.Add("Debug", () => new MySettings()
            {
                CivilWarToggle = true,
                BSIPlotsDebug = true,
            });
            return basePresets;
        }



    }
}