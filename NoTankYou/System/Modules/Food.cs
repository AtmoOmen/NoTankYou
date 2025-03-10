﻿using KamiLib.Caching;
using KamiLib.Utilities;
using Lumina.Excel.GeneratedSheets;
using NoTankYou.Abstracts;
using NoTankYou.Localization;
using NoTankYou.Models.Enums;

namespace NoTankYou.System.Modules;

public class Food : ConsumableModule
{
    public override ModuleName ModuleName => ModuleName.Food;
    public override string DefaultWarningText { get; protected set; } = Strings.FoodWarning;

    protected override uint IconId { get; set; } = LuminaCache<Item>.Instance.GetRow(30482)!.Icon;
    protected override string IconLabel { get; set; } = ModuleName.Food.GetLabel();
    protected override uint StatusId { get; set; } = 48; // Well Fed
}