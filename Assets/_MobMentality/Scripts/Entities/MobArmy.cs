using System;
using MobMentality.Cards;
using MobMentality.Core;

namespace MobMentality.Entities
{
    /// <summary>Owns progression and shared combat stats for the player's mob.</summary>
    public sealed class MobArmy
    {
        private readonly GameEvents gameEvents;

        public MobArmy(Stats stats = null, GameEvents gameEvents = null)
        {
            Stats = stats ?? new Stats(30f, 5f, 1f, 2.2f, 1.1f);
            this.gameEvents = gameEvents;
            Level = 1;
        }

        public int Level { get; private set; }
        public Stats Stats { get; }

        /// <summary>Applies a card intended for the mob army.</summary>
        public void ApplyUpgrade(UpgradeCard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));
            if (card.Target != UpgradeTarget.MobArmy)
                throw new InvalidOperationException("This card does not target the mob army.");

            ApplyEffect(Stats, card.Effect, card.Amount);
            Level++;
            gameEvents?.RaiseCardApplied(card);
            gameEvents?.RaiseMobLeveledUp(Level);
        }

        internal static void ApplyEffect(Stats stats, UpgradeEffect effect, float amount)
        {
            switch (effect)
            {
                case UpgradeEffect.MaxHealth: stats.AddMaxHealth(amount); break;
                case UpgradeEffect.Damage: stats.AddDamage(amount); break;
                case UpgradeEffect.AttackSpeed: stats.AddAttackSpeed(amount); break;
                case UpgradeEffect.MoveSpeed: stats.AddMoveSpeed(amount); break;
                case UpgradeEffect.Range: stats.AddRange(amount); break;
                default: throw new ArgumentOutOfRangeException(nameof(effect));
            }
        }
    }
}
