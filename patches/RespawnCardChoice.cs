using HarmonyLib;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), nameof(CardChoice.IDoEndPick))]
    public static class RespawnCardChoice
    {
        // This class forked from https://github.com/willuwontu/WillsWackyGamemodes/blob/main/WillsWackyGamemodes/Patches/CardChoice_Patch.cs
        class SimpleEnumerator : IEnumerable
        {
            public IEnumerator enumerator;
            public Action prefixAction, postfixAction;
            public Action<object> preItemAction, postItemAction;
            public Func<object, object> itemAction;
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
            public IEnumerator GetEnumerator()
            {
                prefixAction();
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    preItemAction(item);
                    yield return itemAction(item);
                    postItemAction(item);
                }
                postfixAction();
            }
        }

        public static void Postfix(GameObject pickedCard, ref IEnumerator __result, ref List<GameObject> ___spawnedCards)
        {
            var justPickedCards = new List<GameObject>() { pickedCard };
            var spawnedCardsSave = ___spawnedCards;

            // If the state is null or empty, return
            var myEnumerator = new SimpleEnumerator()
            {
                enumerator = __result,
                prefixAction = () => { },
                postfixAction = () => {
                    if (!pickedCard || pickedCard.name.Contains("Continue"))
                    {
                        return;
                    }

                    // Get using reflection
                    var spawnedCards = spawnedCardsSave;

                    // Remove the card from the list of spawned cards
                    spawnedCards.Remove(pickedCard);

                    // Update the ints
                    for (int i = 0; i < spawnedCards.Count; i++)
                    {
                        spawnedCards[i].GetComponentInChildren<PublicInt>().theInt = i;
                    }

                    // Set using reflection
                    typeof(CardChoice).GetField("spawnedCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(CardChoice.instance, spawnedCards);

                    // Set currently selected card
                    typeof(CardChoice).GetField("currentlySelectedCard", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(CardChoice.instance, 0);
                    
                    // Destroy the card
                    pickedCard.GetComponentInChildren<CardVisuals>().Pick();
                },
                preItemAction = item => {
                    if (!pickedCard || pickedCard.name.Contains("Continue"))
                    {
                        return;
                    }

                    // Set using reflection
                    typeof(CardChoice).GetField("spawnedCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(CardChoice.instance, justPickedCards);
                },
                postItemAction = item => {
                    if (!pickedCard || pickedCard.name.Contains("Continue"))
                    {
                        return;
                    }

                    // Set using reflection
                    typeof(CardChoice).GetField("spawnedCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(CardChoice.instance, spawnedCardsSave);
                },
                itemAction = item => item
            };
            __result = myEnumerator.GetEnumerator();
        }
    }
}
