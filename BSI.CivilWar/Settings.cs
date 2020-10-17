
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;

namespace BSI.CivilWar
{
    public class MySettings : AttributeGlobalSettings<MySettings>
    {
        public override string Id => "BSI.CivilWar";
        public override string DisplayName => "BSI - Civil War";
        public override string FolderName => "BSI.CivilWar";
        public override string Format => "json";

        //Main Settings for Wage Model
        [SettingPropertyBool("{=BSIPLOTS_SETTING_01}1. Civil War", HintText = "{=BSIPLOTS_SETTING_DESC_01}Check this to enable Civil Wars", Order = 0, RequireRestart = true)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War", GroupOrder = 0, IsMainToggle = true)]
        public bool CivilWarToggle { get; set; } = true;

        [SettingPropertyInteger("{=BSIPLOTS_SETTING_02}Base Daily Plot Chance", 1, 100, HintText = "{=BSIPLOTS_SETTING_DESC_02}Base Chance for a Lord to plot against his Liege if he meets the requirements", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/1.Plotting Phase", GroupOrder = 0)]
        public int BasePlotChance{ get; set; } = 10;

        [SettingPropertyInteger("{=BSIPLOTS_SETTING_03}Negative Relation Threshold", -100, 100, HintText = "{=BSIPLOTS_SETTING_DESC_03}Threshold at which a Lord can start Plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/1.Plotting Phase", GroupOrder = 0)]
        public int NegativeRelationThreshold { get; set; } = -10;

        [SettingPropertyInteger("{=BSIPLOTS_SETTING_04}Positive Relation Threshold", -100, 100, HintText = "{=BSIPLOTS_SETTING_DESC_04}Threshold at which a Lord will stop Plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/1.Plotting Phase", GroupOrder = 0)]
        public int PositiveRelationThreshold { get; set; } = 10;

        [SettingPropertyFloatingInteger("{=BSIPLOTS_SETTING_05}Plotting Personality Multiplier", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_05}Base Weight given to Lord's individual propensity to engage in a plot", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/1.Plotting Phase", GroupOrder = 0)]
        public float PlotPersonalityMult { get; set; } = 2f;

        [SettingPropertyFloatingInteger("{=BSIPLOTS_SETTING_06}Plotting Friend Mult", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_06}Base weight given to how likely a lord is to be convinced by a friend to start plotting", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/1.Plotting Phase", GroupOrder = 0)]
        public float PlotFriendMult { get; set; } = 2f;

        [SettingPropertyInteger("{=BSIPLOTS_SETTING_07}Base War Chance", 0, 100, HintText = "{=BSIPLOTS_SETTING_DESC_07}Base Chance for a Plot to Declare War", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float WarBaseChance { get; set; } = 5;

        [SettingPropertyFloatingInteger("{=BSIPLOTS_SETTING_08}War Personality Weight", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_08}Base Weight given to Plot Leader's individual propensity to start a war", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float WarPersonalityMult { get; set; } = 2f;

        [SettingPropertyFloatingInteger("{=BSIPLOTS_SETTING_09}War Valor Factor", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_09}Base Weight given to Plot Leader's individual propensity to start a risky war", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float WarValorFactor { get; set; } = 2f;

        [SettingPropertyFloatingInteger("{=BSIPLOTS_SETTING_10}War Calculating Weight", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_10}Base Weight given to Plot Leader's individual propensity to start a war with fewer allies", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float WarLeaderCalculatingMult { get; set; } = 2f;

        [SettingPropertyInteger("{=BSIPLOTS_SETTING_11}Relationship Change between Faction Member", -100, 0, HintText = "{=BSIPLOTS_SETTING_DESC_07}Relationship change between the members of the resulting faction split", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float WarRelationshipChange { get; set; } = -10;

        [SettingPropertyInteger("{=BSIPLOTS_SETTING_11}Relationship Change between Faction Member", 0, 100, HintText = "{=BSIPLOTS_SETTING_DESC_07}Relationship change between the members of the opposing factions", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float AllyRelationshipChange { get; set; } = 10;


        [SettingPropertyFloatingInteger("{=BSIPLOTS_SETTING_12}War Calculating Weight", 0f, 5f, HintText = "{=BSIPLOTS_SETTING_DESC_10}Base Weight given to Plot Leader's individual propensity to start a war with fewer allies", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIPLOTS_SETTING_GROUP_01}1. Civil War/2.War Phase", GroupOrder = 0)]
        public float LeaderRelationshipChangeFactor { get; set; } = 2f;

        //Debug Toggle
        [SettingPropertyBool("{=BSIPLOTS_SETTING_DEBUG}Debug Toggle", HintText = "{=BSIPLOTS_SETTING_DESC_DEBUG}Check this to enable Debug mode", Order = 0, RequireRestart = false)]
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