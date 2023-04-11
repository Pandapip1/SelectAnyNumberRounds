using HarmonyLib;

namespace SelectAnyNumberRounds.Patch
{
    [HarmonyPatch(typeof(CardBar), nameof(CardBar.AddCard))]
    public static class DontAddContinueToBar
    {
        public static bool Prefix(CardInfo card)
        {
            return card.cardName != "Continue"; // Do not add the continue card to the player's hand
        }
    }
}