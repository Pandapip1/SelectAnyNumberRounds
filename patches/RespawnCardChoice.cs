using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), nameof(CardChoice.IDoEndPick))]
    public static class RespawnCardChoice
    {
        public static bool Prefix(GameObject pickedCard, ref List<GameObject> __state, ref CardChoice __instance, ref List<GameObject> ___spawnedCards, ref int ___currentlySelectedCard)
        {
            // If the player picked no card, return true to allow the original method to run
            if (!pickedCard)
            {
                __state = null;
                return true;
            }

            // If the player picked the continue card, clear the list of cards as usual
            if (pickedCard.name.Contains("Continue"))
            {
                __state = null;
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

            // Set the state to the list of spawned cards
            __state = ___spawnedCards;

            // Set the list of spawned cards to the list containing only the picked card
            ___spawnedCards = new List<GameObject>() { pickedCard };

            // Update the currently selected card
            ___currentlySelectedCard = 0;

            // The original method will now run, but it will only pick the card that was picked, and won't destroy the other cards
            return true;
        }

        public static void Postfix(ref List<GameObject> __state, ref List<GameObject> ___spawnedCards)
        {
            // If the state is null or empty, return
            if (__state == null || __state.Count == 0)
            {
                return;
            }

            // Otherwise, restore the list of spawned cards
            ___spawnedCards = __state;
        }
    }
}