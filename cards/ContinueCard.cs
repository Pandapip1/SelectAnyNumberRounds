using UnboundLib.Cards;
using UnityEngine;

namespace SelectAnyNumberRounds.Cards
{
    public class ContinueCard : CustomCard
    {
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
            character.currentCards.RemoveAt(continueIndex);
        }

        public override string GetModName()
        {
            return "Select Any Number";
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }
    }
}