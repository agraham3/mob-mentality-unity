using System;

namespace MobMentality.Waves
{
    /// <summary>Contains the small set of values needed to spawn a wave.</summary>
    [Serializable]
    public sealed class WaveDefinition
    {
        public WaveDefinition(int number, int enemyCount, float enemyHealth, float enemyDamage)
        {
            Number = number;
            EnemyCount = enemyCount;
            EnemyHealth = enemyHealth;
            EnemyDamage = enemyDamage;
        }

        public int Number { get; }
        public int EnemyCount { get; }
        public float EnemyHealth { get; }
        public float EnemyDamage { get; }

        /// <summary>Creates a gently scaling definition for the requested wave.</summary>
        public static WaveDefinition CreateScaled(int number)
        {
            int safeNumber = Math.Max(1, number);
            return new WaveDefinition(safeNumber, 2 + safeNumber, 15f + safeNumber * 4f, 2f + safeNumber * 0.5f);
        }
    }
}
