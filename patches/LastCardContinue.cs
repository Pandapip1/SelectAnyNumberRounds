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
        [HarmonyPriority(Priority.Last)] // Run this patch last, as the contune card MUST NOT be overwritten
        [HarmonyAfter(new string[] { "com.Root.Null" })] // Run this patch after these other Priority.Last patches
        public static void Postfix(ref CardChoice __instance, ref GameObject __result, ref List<GameObject> ___spawnedCards, ref Transform[] ___children, ref int ___pickrID, Vector3 pos, Quaternion rot)
        {
            // If this is the last card, set the result to the continue card
            if (___spawnedCards.Count == ___children.Length - 1) // It's not yet added to the list, so we need to subtract 1
            {
                GameObject old = __result;
                Plugin.instance.ExecuteAfterFrames(3, () => PhotonNetwork.Destroy(old));
                // Spawn the continue card
                __result = PhotonNetwork.Instantiate("__SAN__Continue", __result.transform.position, __result.transform.rotation, 0, new object[] { __result.transform.localScale, __result.GetComponent<CardInfo>().sourceCard.name, ___pickrID });
                __result.GetComponent<CardInfo>().sourceCard = Cards.ContinueCard.cardInfoInstance;
                __result.GetComponentInChildren<DamagableEvent>().GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}