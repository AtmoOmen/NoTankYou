﻿using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Objects.SubKinds;
using KamiLib.Caching;
using KamiLib.Configuration;
using KamiLib.Drawing;
using KamiLib.Misc;
using Lumina.Excel.GeneratedSheets;
using NoTankYou.DataModels;
using NoTankYou.Interfaces;
using NoTankYou.Localization;
using Condition = KamiLib.GameState.Condition;

namespace NoTankYou.Modules;

public class FoodConfiguration : GenericSettings
{
    public Setting<int> FoodEarlyWarningTime = new(600);
    public Setting<bool> SavageDuties = new(false);
    public Setting<bool> UltimateDuties = new(false);
    public Setting<bool> ExtremeUnreal = new(false);
    public Setting<bool> DisableInCombat = new(true);
    public Setting<bool> CriterionDuties = new(false);
    public Setting<bool> EnableZoneFilter = new(false);
}

public class Food : BaseModule
{
    public override ModuleName Name => ModuleName.Food;
    public override string Command => "food";
    public override List<uint> ClassJobs { get; }

    private static FoodConfiguration Settings => Service.ConfigurationManager.CharacterConfiguration.Food;
    public override GenericSettings GenericSettings => Settings;

    private const int WellFedStatusID = 48;
    private readonly Item food;
    
    public Food()
    {
        ClassJobs = LuminaCache<ClassJob>.Instance
            .Select(r => r.RowId)
            .ToList();
            
        food = LuminaCache<Item>.Instance.GetRow(30482)!;
    }

    public override WarningState? EvaluateWarning(PlayerCharacter character)
    {
        if (Settings.DisableInCombat && Condition.IsInCombat()) return null;

        if (Settings.EnableZoneFilter)
        {
            var allowedTypes = new List<DutyType>();
                
            if(Settings.SavageDuties) allowedTypes.Add(DutyType.Savage);
            if(Settings.UltimateDuties) allowedTypes.Add(DutyType.Ultimate);
            if(Settings.ExtremeUnreal) allowedTypes.Add(DutyType.ExtremeUnreal);
            if(Settings.CriterionDuties) allowedTypes.Add(DutyType.Criterion);

            if (!DutyLists.Instance.IsType(Service.ClientState.TerritoryType, allowedTypes))
            {
                return null;
            }
        }
            
        var statusEffect = character.StatusList.FirstOrDefault(status => status.StatusId == WellFedStatusID);
        if (statusEffect == null || statusEffect.RemainingTime < Settings.FoodEarlyWarningTime.Value)
        {
            return new WarningState
            {
                MessageLong = Strings.Food_WarningText,
                MessageShort = Strings.Food_WarningText,
                IconID = food.Icon,
                IconLabel = Strings.Food_Label,
                Priority = Settings.Priority.Value,
            };
        }

        return null;
    }

    protected override void DrawExtraConfiguration()
    {
        InfoBox.Instance
            .AddTitle(Strings.Food_EarlyWarningLabel, out var innerWidth)
            .AddInputInt(Strings.Labels_Seconds, Settings.FoodEarlyWarningTime, 0, 3600, 0, 0, innerWidth / 4.0f)
            .Draw();

        InfoBox.Instance
            .AddTitle(Strings.Common_AdditionalOptions)
            .AddConfigCheckbox(Strings.Food_SuppressInCombat, Settings.DisableInCombat)
            .Draw();

        if (!Settings.EnableZoneFilter)
        {
            InfoBox.Instance
                .AddTitle(Strings.Food_ZoneFilters)
                .AddString(Strings.Food_ZoneFiltersDescription)
                .AddConfigCheckbox(Strings.Food_EnableFilter, Settings.EnableZoneFilter)
                .Draw();
        }
        else
        {
            InfoBox.Instance
                .AddTitle(Strings.Food_ZoneFilters)
                .AddString(Strings.Food_ZoneFiltersDescription)
                .AddConfigCheckbox(Strings.Food_EnableFilter, Settings.EnableZoneFilter)
                .Indent(15)
                .AddConfigCheckbox(Strings.Labels_Savage, Settings.SavageDuties)
                .AddConfigCheckbox(Strings.Labels_Ultimate, Settings.UltimateDuties)
                .AddConfigCheckbox(Strings.Labels_ExtremeUnreal, Settings.ExtremeUnreal)
                .AddConfigCheckbox(Strings.Labels_Criterion, Settings.CriterionDuties)
                .UnIndent(15)
                .Draw();
        }
    }
}