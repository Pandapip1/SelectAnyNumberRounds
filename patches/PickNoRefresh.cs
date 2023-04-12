using HarmonyLib;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), nameof(CardChoice.Pick))]
    public static class PickNoRefresh
    {
        [HarmonyPriority(Priority.Last)] // Run this patch last, as this replaces the original method
        public static bool Prefix(ref CardChoice __instance, GameObject pickedCard, bool clear, ref PickerType ___pickerType, ref List<GameObject> ___spawnedCards)
        {
            // Override the original method with our own
            var playerID = __instance.pickrID;
            if (pickedCard)
            {
                int[] cardIds = new int[___spawnedCards.Count];
                for (int i = 0; i < ___spawnedCards.Count; i++)
                {
                    cardIds[i] = ___spawnedCards[i].GetComponent<PhotonView>().ViewID;
                }
                pickedCard.GetComponentInChildren<ApplyCardStats>().Pick(__instance.pickrID, false, ___pickerType);
                __instance.GetComponent<PhotonView>().RPC("RPCA_DoEndPick", RpcTarget.All, new object[]
                {
                    cardIds,
                    pickedCard.GetComponent<PhotonView>().ViewID,
                    pickedCard.GetComponent<PublicInt>().theInt,
                    __instance.pickrID
                });
            } else if (PlayerManager.instance.players.Find((Player p) => p.playerID == playerID).data.view.IsMine)
            {
                return true; // Let the original method handle this, since we need to actually spawn in cards in the first place
            }
            return false;
        }
    }
}
