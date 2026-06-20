using MobMentality.Cards;
using MobMentality.Core;

namespace MobMentality.Entities
{
    /// <summary>Tracks the rival wizard's automatic progression.</summary>
    public sealed class BossWizard
    {
        private readonly GameEvents gameEvents;

        public BossWizard(Stats stats = null, GameEvents gameEvents = null)
        {
            Stats = stats ?? new Stats(100f, 8f, 0.7f, 0f, 4f);
            this.gameEvents = gameEvents;
            Level = 1;
        }

        public int Level { get; private set; }
        public Stats Stats { get; }

        /// <summary>Applies the predictable boss growth used by the first slice.</summary>
        public void Upgrade()
        {
            Stats.AddMaxHealth(12f);
            Stats.AddDamage(1.5f);
            Level++;
            gameEvents?.RaiseBossUpgraded(Level);
        }

        /// <summary>Applies a targeted boss card for future content and tests.</summary>
        public void ApplyUpgrade(UpgradeCard card)
        {
            if (card == null || card.Target != UpgradeTarget.BossWizard)
                throw new System.InvalidOperationException("A boss upgrade card is required.");

            MobArmy.ApplyEffect(Stats, card.Effect, card.Amount);
            Level++;
            gameEvents?.RaiseCardApplied(card);
            gameEvents?.RaiseBossUpgraded(Level);
        }
    }
}
