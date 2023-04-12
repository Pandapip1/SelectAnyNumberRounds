using HarmonyLib;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SoundImplementation;
using Photon.Pun;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), nameof(CardChoice.IDoEndPick))]
    public static class RespawnCardChoice
    {
        // Note: this only is run when the continue card is NOT picked.
        public static IEnumerator IDoEndPickPatched(GameObject pickedCard, int theInt, int pickId, CardChoice __instance, float ___speed, List<GameObject> ___spawnedCards)
        {
            Plugin.Logger.LogDebug("IDoEndPickPatched called");

            Vector3 startPos = pickedCard.transform.position;
            Vector3 endPos = CardChoiceVisuals.instance.transform.position;
            float c = 0f;
            while (c < 1f)
            {
                CardChoiceVisuals.instance.framesToSnap = 1;
                Vector3 position = Vector3.LerpUnclamped(startPos, endPos, __instance.curve.Evaluate(c));
                pickedCard.transform.position = position;
                __instance.transform.GetChild(theInt).position = position;
                c += Time.deltaTime * ___speed;
                yield return null;
            }
            GamefeelManager.GameFeel((startPos - endPos).normalized * 2f);
            Plugin.Logger.LogDebug("IDoEndPickPatched: Reached checkpoint 0");
            for (int i = 0; i < ___spawnedCards.Count; i++)
            {
                if (___spawnedCards[i])
                {
                    if (___spawnedCards[i].gameObject == pickedCard)
                    {
                        ___spawnedCards[i].GetComponentInChildren<CardVisuals>().Leave();
                    }
                }
            }
            Plugin.Logger.LogDebug("IDoEndPickPatched: Reached checkpoint 1");
            yield return new WaitForSeconds(0.25f);
            AnimationCurve softCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            Vector3 startPos2 = __instance.transform.GetChild(theInt).transform.position;
            Vector3 endPos2 = startPos;
            c = 0f;
            while (c < 1f)
            {
                Vector3 position2 = Vector3.LerpUnclamped(startPos2, endPos2, softCurve.Evaluate(c));
                __instance.transform.GetChild(theInt).position = position2;
                c += Time.deltaTime * ___speed * 1.5f;
                yield return null;
            }
            Plugin.Logger.LogDebug("IDoEndPickPatched: Reached checkpoint 2");
            SoundPlayerStatic.Instance.PlayPlayerBallDisappear();
            __instance.transform.GetChild(theInt).position = startPos;
            yield return null;

            Plugin.Logger.LogDebug("IDoEndPickPatched: Reached checkpoint 3");
            for (int i = 0; i < ___spawnedCards.Count; i++)
            {
                Plugin.Logger.LogDebug($"IDoEndPickPatched: Reached checkpoint 3.1: {i}");
                if (___spawnedCards[i] == pickedCard)
                {
                    ___spawnedCards[i] = null;
                } else if (___spawnedCards[i])
                {
                    if (___spawnedCards[i].GetComponent<PublicInt>())
                    {
                        ___spawnedCards[i].GetComponent<PublicInt>().theInt = i;
                    } else
                    {
                        Plugin.Logger.LogWarning($"Object {___spawnedCards[i].name} does not have a PublicInt component. Adding one now.");
                        ___spawnedCards[i].AddComponent<PublicInt>().theInt = i;
                    }
                }
            }
            Plugin.Logger.LogDebug("IDoEndPickPatched: Reached checkpoint 4");

            // Remove all null entries from the list
            ___spawnedCards.RemoveAll((GameObject x) => x == null);

            // Ensure IsPicking is set to true
            __instance.IsPicking = true;

            // Ensure that pickrID is set to the correct value
            __instance.pickrID = pickId;

            yield break;
        }

        public static bool Prefix(GameObject pickedCard, int theInt, int pickId, CardChoice __instance, float ___speed, List<GameObject> ___spawnedCards, ref IEnumerator __result)
        {
            if (!pickedCard || pickedCard.name == "__SAN__Continue(Clone)")
            {
                return true;
            }
            __result = IDoEndPickPatched(pickedCard, theInt, pickId, __instance, ___speed, ___spawnedCards);
            return false;
        }
    }
}
