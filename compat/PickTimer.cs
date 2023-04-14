using UnityEngine;
using BepInEx;

namespace SelectAnyNumberRounds.Compat
{
    [BepInProcess("Rounds.exe")]
    [BepInDependency(Plugin.pluginId)]
    [BepInDependency("ot.dan.rounds.picktimer")]
    [BepInPlugin("com.pandapip1.rounds.selectanynumberrounds.compat.picktimercompat", "Select Any Number Rounds - Pick Timer Compatibility", PluginInfo.PLUGIN_VERSION)]
    public class PickTimerCompat : BaseUnityPlugin
    {
        public virtual void Start()
        {
            var pickTimerController = typeof(PickTimer.PickTimer).Assembly.GetType("PickTimer.Util.PickTimerController");
            var randomField = pickTimerController.GetField("Random", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            randomField.SetValue(null, new FakeRNG());
        }
    }

    public class FakeRNG : System.Random
    {
        public override int Next()
        {
            return CardChoice.instance.transform.childCount - 1;
        }

        public override int Next(int maxValue)
        {
            return CardChoice.instance.transform.childCount - 1;
        }

        public override int Next(int minValue, int maxValue)
        {
            return CardChoice.instance.transform.childCount - 1;
        }

        public override double NextDouble()
        {
            return CardChoice.instance.transform.childCount - 1;
        }

        public override void NextBytes(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(CardChoice.instance.transform.childCount - 1);
            }
        }
    }
}