using System;
using System.Collections.Generic;
using MobMentality.Cards;
using MobMentality.Entities;
using MobMentality.Waves;

namespace MobMentality.Core
{
    /// <summary>Coordinates the small wave-to-reward loop using plain C# systems.</summary>
    public sealed class GameManager
    {
        public GameManager(int? cardSeed = null)
        {
            Events = new GameEvents();
            Waves = new WaveManager(Events);
            Cards = new CardSystem(Events, cardSeed);
            MobArmy = new MobArmy(gameEvents: Events);
            BossWizard = new BossWizard(gameEvents: Events);
            State = GameState.Ready;
        }

        public GameState State { get; private set; }
        public GameEvents Events { get; }
        public WaveManager Waves { get; }
        public CardSystem Cards { get; }
        public MobArmy MobArmy { get; }
        public BossWizard BossWizard { get; }

        /// <summary>Starts the current wave.</summary>
        public WaveDefinition StartWave()
        {
            if (State != GameState.Ready)
                throw new InvalidOperationException("The game is not ready to start a wave.");

            SetState(GameState.Wave);
            return Waves.StartWave();
        }

        /// <summary>Finishes combat and draws the reward choices.</summary>
        public IReadOnlyList<UpgradeCard> CompleteWave()
        {
            if (State != GameState.Wave)
                throw new InvalidOperationException("There is no active wave to complete.");

            Waves.CompleteWave();
            SetState(GameState.Reward);
            Events.RaiseRewardPhaseStarted();
            return Cards.Draw(3);
        }

        /// <summary>Applies the chosen mob card, upgrades the boss, and readies the next wave.</summary>
        public void ChooseCard(UpgradeCard card)
        {
            if (State != GameState.Reward)
                throw new InvalidOperationException("Cards can only be chosen during the reward phase.");

            MobArmy.ApplyUpgrade(card);
            BossWizard.Upgrade();
            Waves.PrepareNextWave();
            SetState(GameState.Ready);
        }

        private void SetState(GameState state)
        {
            State = state;
            Events.RaiseGameStateChanged(state);
        }
    }
}
