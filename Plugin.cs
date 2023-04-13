using BepInEx;
using UnboundLib.Cards;
using UnboundLib;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using SettingsUI;
using UnityEngine.UI;
using UnboundLib.Utils.UI;
using UnityEngine;
using TMPro;

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
            SettingsUI.RWFSettingsUI.RegisterMenu(PluginInfo.PLUGIN_GUID, NewGUI);
            configUnlimitedPicks = Config.Bind(PluginInfo.PLUGIN_GUID, "Unlimited Picks", true, "If true, you can pick as many cards as you want from each hand. If false, you can only pick the number of cards specified in the 'Picks' setting.");
            configPickNumber = Config.Bind(PluginInfo.PLUGIN_GUID, "Picks", 1, "The number of cards you can pick from each hand.");

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

        internal static void NewGUI(GameObject menu)
        {
            MenuHandler.CreateText(PluginInfo.PLUGIN_NAME + " Options", menu, out TextMeshProUGUI _, 60);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            MenuHandler.CreateToggle(true, "Unlimited Picks", menu, newValue => configUnlimitedPicks.Value = newValue);
            MenuHandler.CreateSlider("Picks", menu, 30, 1f, 20f, 1f, newValue => configPickNumber.Value = (int)newValue, out Slider _, true);
        }
    }
}
