using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using UnboundLib;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), "SpawnUniqueCard")]
    public static class LastCardContinue
    {
        [HarmonyPriority(Priority.Last)] // Run this patch last, as the continue card MUST NOT be overwritten
        [HarmonyAfter(new string[] { "com.Root.Null", "com.willuwontu.rounds.cards", "pykess.rounds.plugins.cardchoicespawnuniquecardpatch", "pykess.rounds.plugins.pickphaseshenanigans" })] // Run this patch after these other Priority.Last patches
        public static void Postfix(ref CardChoice __instance, ref GameObject __result, ref List<GameObject> ___spawnedCards, ref Transform[] ___children, ref int ___pickrID)
        {
            // If this is the last card, set the result to the continue card
            if (___spawnedCards.Count == ___children.Length - 1 && Plugin.enableContinueCard.Value) // It's not yet added to the list, so we need to subtract 1
            {
                GameObject old = __result;
                Plugin.instance.ExecuteAfterFrames(3, () => PhotonNetwork.Destroy(old));
                // Spawn the continue card using reflection
                Plugin.Logger.LogDebug("Spawning continue card");
                __result = (GameObject)typeof(CardChoice).GetMethod("Spawn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(__instance, new object[] { Cards.ContinueCard.cardInfoInstance.gameObject, __result.transform.position, __result.transform.rotation });
                __result.GetComponent<CardInfo>().sourceCard = Cards.ContinueCard.cardInfoInstance;
                __result.GetComponentInChildren<DamagableEvent>().GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
