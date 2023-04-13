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

                // Pick N cards compatibility: apply scale
                var numDraws = ___children.Length;
                var scale = GetScale(numDraws);
                
                __result.transform.localScale = scale;
            }
        }

        // From Pick N Cards
        private const int maxDraws = 20;
        private const float xC = 0.5f;
        private const float yC = 0.5f;
        private const float absMaxX = 0.85f;
        private const float defMaxXWorld = 25f;
        internal const float z = -5f;
        

        public static Vector3 ScreenPoint(this Vector3 v3)
        {
            Vector3 vec = MainCam.instance.transform.GetComponent<Camera>().WorldToScreenPoint(v3);
            vec.x /= (float)Screen.width;
            vec.y /= (float)Screen.height;
            vec.z = 0f;

            return vec;
        }

        public static Vector3 WorldPoint(this Vector3 v3)
        {
            v3.x *= (float)Screen.width;
            v3.y *= (float)Screen.height;
            Vector3 vec = MainCam.instance.transform.GetComponent<Camera>().ScreenToWorldPoint(v3);
            vec.z = z;
            return vec;
        }
        public static float xScreenPoint(this float x)
        {
            return ((new Vector3(x, 0f, 0f)).ScreenPoint()).x;
        }
        public static float xWorldPoint(this float x)
        {
            return ((new Vector3(x, 0f, 0f)).WorldPoint()).x;
        }
        public static float yScreenPoint(this float y)
        {
            return ((new Vector3(0f, y, 0f)).ScreenPoint()).y;
        }
        public static float yWorldPoint(this float y)
        {
            return ((new Vector3(0f, y, 0f)).WorldPoint()).y;
        }
        internal static Vector3 GetScale(int N)
        {
            // camera scale factor
            float factor = 1.04f * absMaxX.xWorldPoint() / defMaxXWorld;

            if (N == 5)
            {
                return new Vector3(1f, 1f, 1f) * factor;
            }
            else if (N < 5)
            {
                return new Vector3(1f, 1f, 1f) * factor * (1f + 1f / (2f*N));
            }
            else if (N > maxDraws / 2)
            {
                return new Vector3(1f, 1f, 1f) * factor * UnityEngine.Mathf.Clamp(5f / (N / 2 + 2), 3f / 5f, 1f);
            }
            else
            {
                return new Vector3(1f, 1f, 1f) * factor * UnityEngine.Mathf.Clamp(5f / (N - 1), 3f / 5f, 1f);
            }

        }
    }
}