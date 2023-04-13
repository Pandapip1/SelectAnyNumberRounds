using BepInEx;
using UnboundLib.Cards;
using UnboundLib;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;

namespace SelectAnyNumberRounds
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static new ManualLogSource Logger => Plugin.instance.GetLogger();

        public static ConfigEntry<bool> configUnlimitedPicks;
        public static ConfigEntry<int> configPickNumber;
        
        private void Awake()
        {
            instance = this;

            // Config
            configUnlimitedPicks = instance.Config.Bind(PluginInfo.PLUGIN_GUID, "Unlimited Picks", true, "If true, you can pick as many cards as you want from each hand. If false, you can only pick the number of cards specified in the 'Picks' setting.");
            configPickNumber = instance.Config.Bind(PluginInfo.PLUGIN_GUID, "Picks", 1, "The number of cards you can pick from each hand.");

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
            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }

        internal ManualLogSource GetLogger()
        {
            return base.Logger;
        }
    }
}
