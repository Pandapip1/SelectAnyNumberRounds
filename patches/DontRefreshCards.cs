using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), "ReplaceCards")]
    public static class DontRefreshCards
    {
        public static bool Prefix(GameObject pickedCard, bool clear, ref IEnumerator<YieldInstruction> __result)
        {
            if (!pickedCard || pickedCard.name.Contains("Continue"))
            {
                return true;
            }
            __result = new List<YieldInstruction>().GetEnumerator(); // Return an empty IEnumerator coroutine
            return false;
        }
    }
}
