namespace ExteriorACU.Configuration;

using Nautilus.Handlers;
using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using Nautilus.Utility;

[Menu(PluginInfo.PLUGIN_NAME, LoadOn = MenuAttribute.LoadEvents.MenuOpened | MenuAttribute.LoadEvents.MenuRegistered,
SaveOn = MenuAttribute.SaveEvents.ChangeValue | MenuAttribute.SaveEvents.SaveGame | MenuAttribute.SaveEvents.QuitGame)]
public class Config : ConfigFile
{
    [Toggle("Enable Exterior ACU")]
    public bool enableExteriorACU = true;

    [Slider("Exterior ACU Creature Limit", 20, 200, DefaultValue = 20, Step = 1)]
    public int exteriorWaterParkSize = 20;

    public static Config Instance { get; private set; }

    internal static void Register()
    {
        if (Instance != null) return;

        Instance = OptionsPanelHandler.RegisterModOptions<Config>();
        Instance.Load();
        SaveUtils.RegisterOnSaveEvent(Instance.Save);
    }

    public static bool EnableExteriorACU => Instance.enableExteriorACU;

    public static int ExteriorWaterParkSize => Instance.exteriorWaterParkSize;

    public static ModOptions OptionsMenu { get; private set; }
}