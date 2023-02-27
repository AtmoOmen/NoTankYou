﻿using System.Collections.Generic;
using System.Diagnostics;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using ImGuiNET;
using KamiLib.Interfaces;
using NoTankYou.UserInterface.Components;

namespace NoTankYou.UserInterface.Tabs;

public class SettingsTab : ISelectionWindowTab
{
    private readonly List<ISelectable> selectables = new()
    {
        new BannerOverlayConfigurationSelectable(),
        new PartyOverlayConfigurationSelectable(),
        new BlacklistConfigurationSelectable(),
    };

    public string TabName => "Settings";
    public ISelectable? LastSelection { get; set; }
    public IEnumerable<ISelectable> GetTabSelectables() => selectables;
    
    public void DrawTabExtras()
    {
        var buttonSize = ImGuiHelpers.ScaledVector2(30.0f);
        var region = ImGui.GetContentRegionAvail();
        
        var cursorStart = ImGui.GetCursorPos();
        cursorStart.X += region.X / 2.0f - buttonSize.X / 2.0f;
        
        ImGui.SetCursorPos(cursorStart);
        
        ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0x005E5BFF);
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, 0xDD000000 | 0x005E5BFFC);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0x005E5BFF);

        if (ImGuiComponents.IconButton("KoFiButton", FontAwesomeIcon.Coffee)) Process.Start(new ProcessStartInfo { FileName = "https://ko-fi.com/midorikami", UseShellExecute = true });
        if (ImGui.IsItemHovered()) ImGui.SetTooltip("Support Me on Ko-Fi");
        
        ImGui.PopStyleColor(3);
    }
}