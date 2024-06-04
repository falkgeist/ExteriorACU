namespace ExteriorACU;

using BepInEx;
using BepInEx.Logging;
using ExteriorACU.BaseRooms;
using HarmonyLib;
using Nautilus;
using Nautilus.Utility;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(Nautilus.PluginInfo.PLUGIN_GUID, Nautilus.PluginInfo.PLUGIN_VERSION)]
[BepInIncompatibility("com.ahk1221.smlhelper")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        Configuration.Config.Register();

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.PLUGIN_GUID);

        // Does what it says
        InitializeExteriorAcuPrefab();

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Loaded {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION}");
    }
    private void InitializeExteriorAcuPrefab()
    {
        ExteriorWaterParkPrefab.Register();
    }
}