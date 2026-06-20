using System;
using MobMentality.Cards;

namespace MobMentality.Core
{
    /// <summary>Publishes the important events in the game loop.</summary>
    public sealed class GameEvents
    {
        public event Action<int> WaveStarted;
        public event Action<int> WaveCompleted;
        public event Action RewardPhaseStarted;
        public event Action<UpgradeCard> CardDrawn;
        public event Action<UpgradeCard> CardApplied;
        public event Action<int> MobLeveledUp;
        public event Action<int> BossUpgraded;
        public event Action<GameState> GameStateChanged;

        /// <summary>Signals that a numbered wave started.</summary>
        public void RaiseWaveStarted(int wave) => WaveStarted?.Invoke(wave);
        /// <summary>Signals that a numbered wave completed.</summary>
        public void RaiseWaveCompleted(int wave) => WaveCompleted?.Invoke(wave);
        /// <summary>Signals that reward choices should be presented.</summary>
        public void RaiseRewardPhaseStarted() => RewardPhaseStarted?.Invoke();
        /// <summary>Signals that a card was drawn.</summary>
        public void RaiseCardDrawn(UpgradeCard card) => CardDrawn?.Invoke(card);
        /// <summary>Signals that a card was applied.</summary>
        public void RaiseCardApplied(UpgradeCard card) => CardApplied?.Invoke(card);
        /// <summary>Signals that the mob reached a new level.</summary>
        public void RaiseMobLeveledUp(int level) => MobLeveledUp?.Invoke(level);
        /// <summary>Signals that the boss reached a new level.</summary>
        public void RaiseBossUpgraded(int level) => BossUpgraded?.Invoke(level);
        /// <summary>Signals that the overall run phase changed.</summary>
        public void RaiseGameStateChanged(GameState state) => GameStateChanged?.Invoke(state);
    }
}
