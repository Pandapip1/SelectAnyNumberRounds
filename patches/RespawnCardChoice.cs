using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), "ReplaceCards")]
    public static class RespawnCardChoice
    {
        public static bool Prefix(ref CardChoice __instance, GameObject pickedCard, ref bool clear, ref List<GameObject> ___spawnedCards, ref int ___currentlySelectedCard)
        {
            // If the player picked no card, return true to allow the original method to run
            if (!pickedCard)
            {
                return true;
            }

            // If the player picked the continue card, clear the list of cards as usual
            if (pickedCard.name.Contains("__SelectAnyNumberRounds__Continue"))
            {
                clear = true;
                return true;
            }

            // Otherwise, partially replace the original method

            // Display the card as picked
            pickedCard.GetComponentInChildren<CardVisuals>().Pick();

            // Remove the card from the list of spawned cards
            ___spawnedCards.Remove(pickedCard);

            // Update the ints
            for (int i = 0; i < ___spawnedCards.Count; i++)
            {
                ___spawnedCards[i].GetComponentInChildren<PublicInt>().theInt = i;
            }

            // Update the currently selected card
            ___currentlySelectedCard = 0;

            // Skip the rest of the method
            return false;
        }
    }
}