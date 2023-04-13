// Fixes the player arm position, which is broken when the indexes don't match.

using HarmonyLib;
using UnityEngine;
using UnboundLib;
using System.Collections.Generic;

namespace SelectAnyNumberRounds.Patch
{
    // Forked from pick N cards' patch
    // Only change from Pick N cards is that the game object is the card itself, not the visual
    [HarmonyPatch(typeof(CardChoiceVisuals), "Update")]
    class CardChoiceVisualsPatchUpdate
    {
        [HarmonyBefore(new string[] { "pykess.rounds.plugins.pickncards" })]
        private static bool Prefix(CardChoiceVisuals __instance)
        {
            __instance.framesToSnap = 1; // Set instant snap

            if (!(bool)__instance.GetFieldValue("isShowinig"))
            {
                return false;
            }
            if (Time.unscaledDeltaTime > 0.1f)
            {
                return false;
            }
            if (__instance.currentCardSelected >= __instance.cardParent.transform.childCount || __instance.currentCardSelected < 0)
            {
                return false;
            }
            if (__instance.rightHandTarget.position.x == float.NaN || __instance.rightHandTarget.position.y == float.NaN || __instance.rightHandTarget.position.z == float.NaN)
            {
                __instance.rightHandTarget.position = Vector3.zero;
                __instance.SetFieldValue("rightHandVel", Vector3.zero);
            }
            if (__instance.leftHandTarget.position.x == float.NaN || __instance.leftHandTarget.position.y == float.NaN || __instance.leftHandTarget.position.z == float.NaN)
            {
                __instance.leftHandTarget.position = Vector3.zero;
                __instance.SetFieldValue("leftHandVel", Vector3.zero);
            }
            GameObject gameObject = ((List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards"))[__instance.currentCardSelected].gameObject;
            Vector3 vector = gameObject.transform.GetChild(0).position;
            if (vector.x < 0f) // it was literally this simple Landfall...
            {
                __instance.SetFieldValue("leftHandVel", (Vector3)__instance.GetFieldValue("leftHandVel") + (vector - __instance.leftHandTarget.position) * __instance.spring * Time.unscaledDeltaTime);
                __instance.SetFieldValue("leftHandVel", (Vector3)__instance.GetFieldValue("leftHandVel") - ((Vector3)__instance.GetFieldValue("leftHandVel")) * Time.unscaledDeltaTime * __instance.drag);
                __instance.SetFieldValue("rightHandVel", (Vector3)__instance.GetFieldValue("rightHandVel") + (((Vector3)__instance.GetFieldValue("rightHandRestPos")) - __instance.rightHandTarget.position) * __instance.spring * Time.unscaledDeltaTime * 0.5f);
                __instance.SetFieldValue("rightHandVel", (Vector3)__instance.GetFieldValue("rightHandVel") - ((Vector3)__instance.GetFieldValue("rightHandVel")) * Time.unscaledDeltaTime * __instance.drag * 0.5f);
                __instance.SetFieldValue("rightHandVel", (Vector3)__instance.GetFieldValue("rightHandVel") + __instance.sway * new Vector3(-0.5f + Mathf.PerlinNoise(Time.unscaledTime * __instance.swaySpeed, 0f), -0.5f + Mathf.PerlinNoise(Time.unscaledTime * __instance.swaySpeed + 100f, 0f), 0f) * Time.unscaledDeltaTime);
                __instance.shieldGem.transform.position = __instance.rightHandTarget.position;
                if (__instance.framesToSnap > 0)
                {
                    __instance.leftHandTarget.position = vector;
                }
            }
            else
            {
                __instance.SetFieldValue("rightHandVel", (Vector3)__instance.GetFieldValue("rightHandVel") + (vector - __instance.rightHandTarget.position) * __instance.spring * Time.unscaledDeltaTime);
                __instance.SetFieldValue("rightHandVel", (Vector3)__instance.GetFieldValue("rightHandVel") - ((Vector3)__instance.GetFieldValue("rightHandVel")) * Time.unscaledDeltaTime * __instance.drag);
                __instance.SetFieldValue("leftHandVel", (Vector3)__instance.GetFieldValue("leftHandVel") + (((Vector3)__instance.GetFieldValue("leftHandRestPos")) - __instance.leftHandTarget.position) * __instance.spring * Time.unscaledDeltaTime * 0.5f);
                __instance.SetFieldValue("leftHandVel", (Vector3)__instance.GetFieldValue("leftHandVel") - ((Vector3)__instance.GetFieldValue("leftHandVel")) * Time.unscaledDeltaTime * __instance.drag * 0.5f);
                __instance.SetFieldValue("leftHandVel", (Vector3)__instance.GetFieldValue("leftHandVel") + __instance.sway * new Vector3(-0.5f + Mathf.PerlinNoise(Time.unscaledTime * __instance.swaySpeed, Time.unscaledTime * __instance.swaySpeed), -0.5f + Mathf.PerlinNoise(Time.unscaledTime * __instance.swaySpeed + 100f, Time.unscaledTime * __instance.swaySpeed + 100f), 0f) * Time.unscaledDeltaTime);
                __instance.shieldGem.transform.position = __instance.leftHandTarget.position;
                if (__instance.framesToSnap > 0)
                {
                    __instance.rightHandTarget.position = vector;
                }
            }
            __instance.framesToSnap--;
            __instance.leftHandTarget.position += (Vector3)__instance.GetFieldValue("leftHandVel") * Time.unscaledDeltaTime;
            __instance.rightHandTarget.position += (Vector3)__instance.GetFieldValue("rightHandVel") * Time.unscaledDeltaTime;

            return false; // skip original (BAD IDEA)
        }
    }
}
