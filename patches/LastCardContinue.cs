using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), "SpawnUniqueCard")]
    public static class LastCardContinue
    {
        [HarmonyPriority(Priority.Last)] // Run this patch first, as the contune card MUST NOT be overwritten
        public static void Postfix(ref CardChoice __instance, ref GameObject __result, ref List<GameObject> ___spawnedCards, ref Transform[] ___children)
        {
            // If this is the last card, set the result to the continue card
            if (___spawnedCards.Count == ___children.Length - 1) // It's not yet added to the list, so we need to subtract 1
            {
                var pos = __result.transform.position;
                var rot = __result.transform.rotation;
                Object.Destroy(__result);
                __result = PhotonNetwork.Instantiate("__SAN__Continue", pos, rot, 0, null);
                __result.GetComponent<CardInfo>().sourceCard = Cards.ContinueCard.cardInfoInstance;
                __result.GetComponentInChildren<DamagableEvent>().GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}