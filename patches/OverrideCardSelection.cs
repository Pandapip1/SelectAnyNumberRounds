using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

// Overrides the card choice so that a "continue" card is always available
namespace SelectAnyNumberRounds.Patch {
    [HarmonyPatch(typeof(CardChoice), "SpawnUniqueCard")]
    public static class OverrideCardSelection {
        // If it picks a continue card, pick a random card instead
        public static void Postfix(ref CardChoice __instance) {
            // Using reflection to access private fields
            var spawnedCards = (List<GameObject>) typeof(CardChoice).GetField("spawnedCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(__instance);
            var cards = (CardInfo[]) typeof(CardChoice).GetField("cards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(__instance);
            
            // If the last card is not a continue card, then make it a continue card
            if (!spawnedCards[spawnedCards.Count - 1].name.Contains("Continue")) {
                // Get data
                Vector3 ogPos = spawnedCards[spawnedCards.Count - 1].transform.position;
                Quaternion ogRot = spawnedCards[spawnedCards.Count - 1].transform.rotation;

                // Find the card
                CardInfo cardInfo = null;
                foreach (CardInfo card in cards) {
                    if (card.cardName == "Continue") {
                        cardInfo = card;
                        break;
                    }
                }

                // Destroy the old card
                PhotonNetwork.Destroy(spawnedCards[spawnedCards.Count - 1]);

                // Make GameObject
                GameObject continueCard = PhotonNetwork.Instantiate(cardInfo.gameObject.name, ogPos, ogRot);
                continueCard.GetComponent<CardInfo>().sourceCard = cardInfo;
                continueCard.GetComponent<DamagableEvent>().GetComponent<Collider2D>().enabled = false;
                spawnedCards[spawnedCards.Count - 1] = continueCard;
            }

            // Remove any other continue cards
            for (int i = 0; i < spawnedCards.Count - 1; i++) {
                if (spawnedCards[i].name.Contains("Continue")) {
                    PhotonNetwork.Destroy(spawnedCards[i]);
                    spawnedCards.RemoveAt(i);
                    i--;
                }
            }

            // Then, set the modified array
            typeof(CardChoice).GetField("spawnedCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(__instance, spawnedCards);
        }
    }
}