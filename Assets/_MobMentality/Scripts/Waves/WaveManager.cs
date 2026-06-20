using System;
using MobMentality.Core;

namespace MobMentality.Waves
{
    /// <summary>Owns wave progression without depending on Unity scene objects.</summary>
    public sealed class WaveManager
    {
        private readonly GameEvents gameEvents;

        public WaveManager(GameEvents gameEvents = null)
        {
            this.gameEvents = gameEvents;
            CurrentWave = 1;
            State = WaveState.Ready;
        }

        public int CurrentWave { get; private set; }
        public WaveState State { get; private set; }

        /// <summary>Starts the current wave and returns its spawn definition.</summary>
        public WaveDefinition StartWave()
        {
            if (State != WaveState.Ready)
                throw new InvalidOperationException("A wave can only start from the Ready state.");

            State = WaveState.Active;
            gameEvents?.RaiseWaveStarted(CurrentWave);
            return WaveDefinition.CreateScaled(CurrentWave);
        }

        /// <summary>Completes the active wave and enters the reward state.</summary>
        public void CompleteWave()
        {
            if (State != WaveState.Active)
                throw new InvalidOperationException("Only an active wave can be completed.");

            State = WaveState.Reward;
            gameEvents?.RaiseWaveCompleted(CurrentWave);
        }

        /// <summary>Advances to the next wave after a reward has been resolved.</summary>
        public void PrepareNextWave()
        {
            if (State != WaveState.Reward)
                throw new InvalidOperationException("The next wave can only be prepared after a reward.");

            CurrentWave++;
            State = WaveState.Ready;
        }
    }
}
