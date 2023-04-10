using UnboundLib.Cards;
using UnityEngine;

namespace SelectAnyNumberRounds.Cards
{
    public class ContinueCard : CustomCard
    {
        public static CardInfo cardInfoInstance = null;

        protected override string GetTitle()
        {
            return "Continue";
        }

        protected override string GetDescription()
        {
            return "I'm done, let's move on!";
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[0];
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo ammo, CharacterData character, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers stats)
        {
            // Remove card from hand
            var continueIndex = character.currentCards.FindIndex(c => c.cardName == "Continue");
            if (continueIndex == -1)
            {
                return;
            }
            character.currentCards.RemoveAt(continueIndex);
        }

        public override string GetModName()
        {
            return "SAN";
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }

        public override bool GetEnabled()
        {
            return false; // Do not show this card in the card selection menu, and do not allow it to be added to the player's hand by normal means
        }
    }
}