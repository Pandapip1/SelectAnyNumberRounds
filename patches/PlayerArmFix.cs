// Fixes the player arm position, which is broken when the indexes don't match.

using HarmonyLib;
using UnityEngine;
using UnboundLib;

namespace SelectAnyNumberRounds.Patch
{
    // Forked from pick N cards' patch
    [HarmonyPatch(typeof(CardChoiceVisuals), "Update")]
    class CardChoiceVisualsPatchUpdate
    {
        [HarmonyBefore(new string[] { "pykess.rounds.plugins.pickncards" })]
        private static void Prefix(CardChoiceVisuals __instance, ref int __state)
        {
            // Store the current card selected
            __state = __instance.currentCardSelected;
            // For each pick ID below currentCardSelected, increase the index by 1
            // This ensures that the player arm is always on the correct card
            var currentCardSelected = __instance.currentCardSelected;
            foreach (var pickId in RespawnCardChoice.rawPickIds)
            {
                if (pickId < currentCardSelected)
                {
                    currentCardSelected++;
                }
            }
            __instance.currentCardSelected = currentCardSelected;
        }

        private static void Postfix(CardChoiceVisuals __instance, int __state)
        {
            // Restore the current card selected
            __instance.currentCardSelected = __state;
        }
    }
}
