using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using UnboundLib;
using System;
using static DrawNCards.WorldToScreenExtensions;
using BepInEx.Configuration;
using System.Linq;

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
            if (___spawnedCards.Count == ___children.Length - 1) // It's not yet added to the list, so we need to subtract 1
            {
                GameObject old = __result;
                Plugin.instance.ExecuteAfterFrames(3, () => PhotonNetwork.Destroy(old));
                // Pick N cards: get positions, angle, and rotations using reflection
                var poses = (List<Vector3>)typeof(DrawNCards.DrawNCards).GetMethod("GetPositions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { ___children.Length, 0f });
                var rots = (List<Quaternion>)typeof(DrawNCards.DrawNCards).GetMethod("GetRotations", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { ___children.Length });
                var scale = (Vector3)typeof(DrawNCards.DrawNCards).GetMethod("GetScale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { ___children.Length });
                var pos = poses[poses.Count - 1];
                var rot = rots[rots.Count - 1];
                // Spawn the continue card
                __result = PhotonNetwork.Instantiate("__SAN__Continue", pos, rot, 0, new object[] { scale, __result.GetComponent<CardInfo>().sourceCard.name, ___pickrID });
                __result.GetComponent<CardInfo>().sourceCard = Cards.ContinueCard.cardInfoInstance;
                __result.GetComponentInChildren<DamagableEvent>().GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}