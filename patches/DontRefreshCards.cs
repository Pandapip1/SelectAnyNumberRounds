using HarmonyLib;
using UnityEngine;
using System.Collections;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), "ReplaceCards")]
    public static class DontRefreshCards
    {
        public static bool Prefix(GameObject pickedCard, bool clear, ref IEnumerator __result)
        {
            if (!pickedCard || pickedCard.name.Contains("Continue"))
            {
                return true;
            }
            __result = GetEnumerator();
            return false;
        }
        
        internal static IEnumerator GetEnumerator()
        {
            yield return null;
        }
    }
}
