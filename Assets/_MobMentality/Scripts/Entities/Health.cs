using System;

namespace MobMentality.Entities
{
    /// <summary>Tracks damage and healing independently of Unity components.</summary>
    public sealed class Health
    {
        public Health(float maximum)
        {
            Maximum = Math.Max(1f, maximum);
            Current = Maximum;
        }

        public float Current { get; private set; }
        public float Maximum { get; private set; }
        public bool IsDead => Current <= 0f;

        /// <summary>Reduces health by a non-negative amount.</summary>
        public void Damage(float amount) => Current = Math.Max(0f, Current - Math.Max(0f, amount));

        /// <summary>Restores health without exceeding the maximum.</summary>
        public void Heal(float amount) => Current = Math.Min(Maximum, Current + Math.Max(0f, amount));
    }
}
