using BepInEx;
using UnboundLib.Cards;
using UnboundLib;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine.UI;
using UnboundLib.Utils.UI;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnboundLib.Networking;

namespace SelectAnyNumberRounds
{
    [BepInPlugin(pluginId, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string pluginId = "com.pandapip1.rounds.selectanynumberrounds";
        public static Plugin instance;
        public static new ManualLogSource Logger => Plugin.instance.GetLogger();

        public static ConfigEntry<int> configPickNumber;
        
        private void Awake()
        {
            instance = this;

            // Config
            SettingsUI.RWFSettingsUI.RegisterMenu(PluginInfo.PLUGIN_GUID, NewGUI);
            configPickNumber = Config.Bind(PluginInfo.PLUGIN_GUID, "Picks", 20, "The number of cards you can pick from each hand.");

            // Sync
            Unbound.RegisterHandshake(PluginInfo.PLUGIN_GUID, this.OnHandShakeCompleted);

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
            harmony.PatchAll(typeof(Patch.AutoLoadPatch));
        }

        internal ManualLogSource GetLogger()
        {
            return base.Logger;
        }

        internal static void NewGUI(GameObject menu)
        {
            MenuHandler.CreateText(PluginInfo.PLUGIN_NAME + " Options", menu, out TextMeshProUGUI _, 60);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            MenuHandler.CreateSlider("Picks", menu, 30, 1f, 20f, 1f, newValue => configPickNumber.Value = (int)newValue, out Slider _, true);
        }

        private void OnHandShakeCompleted()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UnityEngine.Debug.Log("Sending Handshake RPC");
                NetworkingManager.RPC(typeof(Plugin), nameof(SyncSettings), new object[] {
                    configPickNumber.Value
                });
            }
        }

        [UnboundRPC]
        private static void SyncSettings(int draws)
        {
            configPickNumber.Value = draws;
        }
    }
}
