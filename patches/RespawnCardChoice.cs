using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), "ReplaceCards")]
    public static class RespawnCardChoice
    {
        public static bool Prefix(ref CardChoice __instance, GameObject pickedCard, ref bool clear)
        {
            // Ensure the player has picked a card
            if (!pickedCard)
            {
                return true;
            }

            // Get reflection info for the cards list
            // Using reflection to access private fields
            var spawnedCards = (List<GameObject>) typeof(CardChoice).GetField("spawnedCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(__instance);
            var cards = (CardInfo[]) typeof(CardChoice).GetField("cards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(__instance);

            // Fetch continue card info
            CardInfo cardInfo = null;
            foreach (CardInfo card in cards)
            {
                if (card.cardName == "Continue")
                {
                    cardInfo = card;
                    break;
                }
            }

            // If the player picked the continue card, clear the list of cards as usual
            if (pickedCard.GetComponent<CardInfo>().sourceCard == cardInfo)
            {
                clear = true;
                return true;
            }

            // Otherwise, we have to partially replicate the original method
            
            // Play card pick animation
            pickedCard.GetComponentInChildren<CardVisuals>().Pick();

            // Remove the card from the list of cards spawned
            spawnedCards.Remove(pickedCard);

            // Update the other cards
            for (int i = 0; i < spawnedCards.Count; i++)
            {
                spawnedCards[i].GetComponent<PublicInt>().theInt = i;
            }

            // Reflection to put the spawnedCards list back into the CardChoice instance
            typeof(CardChoice).GetField("spawnedCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(__instance, spawnedCards);

            // And now we can return false to prevent the original method from running
            return false;
        }
    }
}