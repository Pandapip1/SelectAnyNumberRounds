using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), "SpawnUniqueCard")]
    public static class CardPositionKluddge
    {
        public static Vector3[] cardPositions;

        [HarmonyPriority(Priority.First)] // Run this patch first, because if other mods override card positions, we want theirs to be used
        [HarmonyPrefix]
        public static void FirstPrefix(CardChoice __instance)
        {
            if (cardPositions == null)
            {
                cardPositions = new Vector3[__instance.transform.childCount];
            }
            for (int i = 0; i < __instance.transform.childCount; i++)
            {
                var child = __instance.transform.GetChild(i);
                if (child != null)
                {
                    if (!(cardPositions[i] == Vector3.zero || cardPositions[i] == null))
                    {
                        child.position = cardPositions[i];
                    }
                }
            }
        }

        [HarmonyPriority(Priority.Last)] // Run this patch last, because if other mods override card positions, we want theirs to be used
        [HarmonyPrefix]
        public static void LastPrefix(CardChoice __instance)
        {
            for (int i = 0; i < __instance.transform.childCount; i++)
            {
                var child = __instance.transform.GetChild(i);
                if (child != null)
                {
                    if (cardPositions[i] == Vector3.zero || cardPositions[i] == null)
                    {
                        cardPositions[i] = child.position;
                    }
                }
            }
        }
    }
}
