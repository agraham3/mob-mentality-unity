using System;

namespace MobMentality.Entities
{
    /// <summary>Tracks experience and supports gaining multiple levels at once.</summary>
    public sealed class Experience
    {
        private readonly Func<int, int> thresholdForLevel;

        public Experience(Func<int, int> thresholdForLevel = null)
        {
            this.thresholdForLevel = thresholdForLevel ?? (level => level * 100);
            Level = 1;
        }

        public int Level { get; private set; }
        public int Current { get; private set; }
        public int Needed => Math.Max(1, thresholdForLevel(Level));

        /// <summary>Adds experience and returns the number of levels gained.</summary>
        public int Add(int amount)
        {
            Current += Math.Max(0, amount);
            int levelsGained = 0;
            while (Current >= Needed)
            {
                Current -= Needed;
                Level++;
                levelsGained++;
            }

            return levelsGained;
        }
    }
}
