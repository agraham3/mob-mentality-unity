using System;

namespace MobMentality.Entities
{
    /// <summary>Stores combat values shared by mobs, enemies, and the boss.</summary>
    [Serializable]
    public sealed class Stats
    {
        public Stats(float maxHealth, float damage, float attackSpeed, float moveSpeed, float range)
        {
            MaxHealth = maxHealth;
            Damage = damage;
            AttackSpeed = attackSpeed;
            MoveSpeed = moveSpeed;
            Range = range;
        }

        public float MaxHealth { get; private set; }
        public float Damage { get; private set; }
        public float AttackSpeed { get; private set; }
        public float MoveSpeed { get; private set; }
        public float Range { get; private set; }
        public float Strength => MaxHealth + Damage * 5f + AttackSpeed * 10f;

        /// <summary>Adds to maximum health while preserving a valid minimum.</summary>
        public void AddMaxHealth(float value) => MaxHealth = Math.Max(1f, MaxHealth + value);
        /// <summary>Adds to attack damage while preserving a valid minimum.</summary>
        public void AddDamage(float value) => Damage = Math.Max(0f, Damage + value);
        /// <summary>Adds attacks per second while preserving a valid minimum.</summary>
        public void AddAttackSpeed(float value) => AttackSpeed = Math.Max(0.1f, AttackSpeed + value);
        /// <summary>Adds to movement speed while preserving a valid minimum.</summary>
        public void AddMoveSpeed(float value) => MoveSpeed = Math.Max(0f, MoveSpeed + value);
        /// <summary>Adds to attack range while preserving a valid minimum.</summary>
        public void AddRange(float value) => Range = Math.Max(0.1f, Range + value);

        /// <summary>Creates a copy that can be changed independently.</summary>
        public Stats Clone() => new Stats(MaxHealth, Damage, AttackSpeed, MoveSpeed, Range);
    }
}
