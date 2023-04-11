using HarmonyLib;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), nameof(CardChoice.Pick))]
    public static class PickNoRefresh
    {
        public static bool Prefix(ref CardChoice __instance, GameObject pickedCard, bool clear, ref PickerType ___pickerType, ref List<GameObject> ___spawnedCards)
        {
            // Override the original method with our own
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
            }
            return false;
        }
    }
}
