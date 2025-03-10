﻿using Dalamud.Plugin;
using KamiLib;
using KamiLib.Commands;
using NoTankYou.Localization;
using NoTankYou.System;
using NoTankYou.Views.Windows;

namespace NoTankYou;

public sealed class NoTankYouPlugin : IDalamudPlugin
{
    public string Name => "NoTankYou";
    
    public static NoTankYouSystem System = null!;

    public NoTankYouPlugin(DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();

        KamiCommon.Initialize(pluginInterface, Name);
        KamiCommon.RegisterLocalizationHandler(key => Strings.ResourceManager.GetString(key, Strings.Culture));

        System = new NoTankYouSystem();
        
        CommandController.RegisterMainCommand("/nty", "/notankyou");
        
        KamiCommon.WindowManager.AddConfigurationWindow(new ConfigurationWindow());
    }
        
    public void Dispose()
    {
        KamiCommon.Dispose();
        
        System.Dispose();
    }
}