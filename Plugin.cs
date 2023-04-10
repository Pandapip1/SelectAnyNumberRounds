using BepInEx;
using UnboundLib.Cards;
using UnboundLib;
using BepInEx.Logging;

namespace SelectAnyNumberRounds
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static new ManualLogSource Logger => Plugin.instance.GetLogger();
        private void Awake()
        {
            instance = this;

            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            // Load credits
            Unbound.RegisterCredits("Pick Any Number", new string[] { "Pandapip1 (@Pandapip1)" }, "Pandapip1.com", "https://pandapip1.com/");

            // Load custom cards
            CustomCard.BuildCard<Cards.ContinueCard>((card) => {
                // Save the CardInfo instance for later use
                Cards.ContinueCard.cardInfoInstance = card;
            });

            // Harmony patching: all patches in the same assembly as this class will be applied
            HarmonyLib.Harmony harmony = HarmonyLib.Harmony.CreateAndPatchAll(typeof(Plugin).Assembly);
        }

        internal ManualLogSource GetLogger()
        {
            return base.Logger;
        }
    }
}
