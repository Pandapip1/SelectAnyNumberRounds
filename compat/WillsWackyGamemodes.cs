using BepInEx;
using UnboundLib.GameModes;

namespace SelectAnyNumberRounds.Compat
{
    [BepInProcess("Rounds.exe")]
    [BepInDependency(Plugin.pluginId)]
    [BepInDependency("com.willuwontu.rounds.gamemodes")]
    [BepInPlugin("com.pandapip1.rounds.selectanynumberrounds.compat.willswackygmcompat", "Select Any Number Rounds - Will's Wacky Gamemodes Compatibility", PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Rounds.exe")]
    public class WillsWackyGMCompat : BaseUnityPlugin
    {
        public static bool oldIsDraft = false;
        public static int oldConfigPickNumber = 0;
        public static bool oldEnableContinueCard = false;
        public virtual void Update()
        {
            if (GameModeManager.CurrentHandlerID == "Draft" && !oldIsDraft)
            {
                oldConfigPickNumber = Plugin.configPickNumber.Value;
                oldEnableContinueCard = Plugin.enableContinueCard.Value;
                Plugin.configPickNumber.Value = 1; // Draft only allows 1 pick
                Plugin.enableContinueCard.Value = false; // Draft doesn't allow continue cards
            }
            if (GameModeManager.CurrentHandlerID != "Draft" && oldIsDraft)
            {
                Plugin.configPickNumber.Value = oldConfigPickNumber;
                Plugin.enableContinueCard.Value = oldEnableContinueCard;
                oldConfigPickNumber = 0;
                oldEnableContinueCard = false;
            }
            oldIsDraft = GameModeManager.CurrentHandlerID == "Draft";
        }

        // Before game quits, reset the config values
        public virtual void OnApplicationQuit()
        {
            if (oldIsDraft)
            {
                Plugin.configPickNumber.Value = oldConfigPickNumber;
                Plugin.enableContinueCard.Value = oldEnableContinueCard;
            }
        }
    }
}
