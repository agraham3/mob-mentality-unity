using System;
using System.Collections.Generic;
using MobMentality.Core;

namespace MobMentality.Cards
{
    /// <summary>Draws distinct upgrade choices from a small placeholder deck.</summary>
    public sealed class CardSystem
    {
        private readonly List<UpgradeCard> deck;
        private readonly Random random;
        private readonly GameEvents gameEvents;

        public CardSystem(GameEvents gameEvents = null, int? seed = null)
        {
            this.gameEvents = gameEvents;
            random = seed.HasValue ? new Random(seed.Value) : new Random();
            deck = CreateStarterDeck();
        }

        /// <summary>Draws up to the requested number of unique cards.</summary>
        public IReadOnlyList<UpgradeCard> Draw(int count = 3)
        {
            var candidates = new List<UpgradeCard>(deck);
            var result = new List<UpgradeCard>();
            int drawCount = Math.Min(Math.Max(0, count), candidates.Count);

            for (int i = 0; i < drawCount; i++)
            {
                int index = random.Next(candidates.Count);
                UpgradeCard card = candidates[index];
                candidates.RemoveAt(index);
                result.Add(card);
                gameEvents?.RaiseCardDrawn(card);
            }

            return result;
        }

        private static List<UpgradeCard> CreateStarterDeck()
        {
            return new List<UpgradeCard>
            {
                new UpgradeCard("Thick Skins", "+10 mob health", UpgradeEffect.MaxHealth, 10f, UpgradeTarget.MobArmy, CardRarity.Common),
                new UpgradeCard("Sharp Sticks", "+2 mob damage", UpgradeEffect.Damage, 2f, UpgradeTarget.MobArmy, CardRarity.Common),
                new UpgradeCard("Battle Rhythm", "+0.2 attacks/second", UpgradeEffect.AttackSpeed, 0.2f, UpgradeTarget.MobArmy, CardRarity.Uncommon),
                new UpgradeCard("Quick Feet", "+0.4 move speed", UpgradeEffect.MoveSpeed, 0.4f, UpgradeTarget.MobArmy, CardRarity.Common),
                new UpgradeCard("Long Reach", "+0.25 attack range", UpgradeEffect.Range, 0.25f, UpgradeTarget.MobArmy, CardRarity.Rare)
            };
        }
    }
}
