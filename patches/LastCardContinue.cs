using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using UnboundLib.Cards;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), "GetRanomCard")] // [sic]
    public static class LastCardContinue
    {
        public static bool Prefix(ref CardChoice __instance, ref GameObject __result, ref List<GameObject> ___spawnedCards, ref Transform[] ___children)
        {
            // If there is only one card left, return the continue card
            if (___spawnedCards.Count == ___children.Length - 1)
            {
                __result = Cards.ContinueCard.cardInfoInstance.gameObject;
                return false;
            }

            // Otherwise, return the original method
            return true;
        }
    }
}