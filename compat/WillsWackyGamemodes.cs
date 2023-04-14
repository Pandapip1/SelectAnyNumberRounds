using BepInEx;
using UnboundLib.GameModes;

namespace SelectAnyNumberRounds.Compat
{
    [BepInProcess("Rounds.exe")]
    [BepInDependency(Plugin.pluginId)]
    [BepInDependency("com.willuwontu.rounds.gamemodes")]
    [BepInPlugin("com.pandapip1.rounds.selectanynumberrounds.compat.willswackygmcompat", "Select Any Number Rounds - Will's Wacky Gamemodes Compatibility", PluginInfo.PLUGIN_VERSION)]
    public class WillsWackyGMCompat : BaseUnityPlugin
    {
        public static int oldConfigPickNumber = 0;
        public virtual void Update()
        {
            if (GameModeManager.CurrentHandlerID == "Draft")
            {
                if (Plugin.configPickNumber.Value != 1)
                {
                    oldConfigPickNumber = Plugin.configPickNumber.Value;
                    Plugin.configPickNumber.Value = 1; // Draft only allows 1 pick
                }
            } else if (oldConfigPickNumber != 0)
            {
                Plugin.configPickNumber.Value = oldConfigPickNumber;
                oldConfigPickNumber = 0;
            }
        }
    }
}
