using HarmonyLib;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SoundImplementation;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardChoice), nameof(CardChoice.IDoEndPick))]
    public static class RespawnCardChoice
    {
        // Note: this only is run when the continue card is NOT picked.
        public static IEnumerator IDoEndPickPatched(GameObject pickedCard, int theInt, int pickId, CardChoice __instance, float ___speed, List<GameObject> ___spawnedCards)
        {
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
            for (int i = 0; i < ___spawnedCards.Count; i++)
            {
                var card = ___spawnedCards[i];
                if (card)
                {
                    if (card.gameObject != pickedCard)
                    {
                        card.AddComponent<Rigidbody>().AddForce((card.transform.position - endPos) * Random.Range(0f, 50f));
                        card.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * Random.Range(0f, 200f));
                        card.AddComponent<RemoveAfterSeconds>().seconds = Random.Range(0.5f, 1f);
                        card.GetComponent<RemoveAfterSeconds>().shrink = true;
                    }
                    //else
                    //{
                    //    card.GetComponentInChildren<CardVisuals>().Leave();
                    //}
                }
            }
            yield return new WaitForSeconds(0.25f);
            AnimationCurve softCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            Transform theIntTransform = __instance.transform.GetChild(theInt); // This used base, but I changed it to __instance. I don't know if that's correct.
            Vector3 startPos2 = theIntTransform.transform.position;
            Vector3 endPos2 = startPos;
            c = 0f;
            while (c < 1f)
            {
                Vector3 position2 = Vector3.LerpUnclamped(startPos2, endPos2, softCurve.Evaluate(c));
                theIntTransform.position = position2;
                c += Time.deltaTime * ___speed * 1.5f;
                yield return null;
            }
            SoundPlayerStatic.Instance.PlayPlayerBallDisappear();
            theIntTransform.position = startPos;
            ___spawnedCards.Clear();
            // Not needed since we're not using the continue card.
            //if (PlayerManager.instance.GetPlayerWithID(pickId).data.view.IsMine)
            //{
            //    base.StartCoroutine(this.ReplaceCards(pickedCard, false));
            //}
            pickedCard.GetComponentInChildren<CardVisuals>().Pick(); // The only important line from the original method.
            yield break;
        }

        public static bool Prefix(GameObject pickedCard, int theInt, int pickId, CardChoice __instance, float ___speed, List<GameObject> ___spawnedCards, ref IEnumerator __result)
        {
            if (!pickedCard || pickedCard.name.Contains("Continue"))
            {
                return true;
            }
            __result = IDoEndPickPatched(pickedCard, theInt, pickId, __instance, ___speed, ___spawnedCards);
            return false;
        }
    }
}
