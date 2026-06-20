using MobMentality.Cards;
using MobMentality.Core;
using MobMentality.Entities;
using MobMentality.Waves;
using NUnit.Framework;

namespace MobMentality.Tests
{
    /// <summary>Verifies the Unity-independent rules used by the first playable slice.</summary>
    public sealed class CoreFrameworkTests
    {
        /// <summary>Verifies the initial wave state.</summary>
        [Test]
        public void WaveManagerStartsReadyAtWaveOne()
        {
            var waves = new WaveManager();

            Assert.That(waves.CurrentWave, Is.EqualTo(1));
            Assert.That(waves.State, Is.EqualTo(WaveState.Ready));
        }

        /// <summary>Verifies the transition into active combat.</summary>
        [Test]
        public void StartingWaveChangesStateToActive()
        {
            var waves = new WaveManager();

            WaveDefinition definition = waves.StartWave();

            Assert.That(waves.State, Is.EqualTo(WaveState.Active));
            Assert.That(definition.Number, Is.EqualTo(1));
        }

        /// <summary>Verifies wave completion opens rewards.</summary>
        [Test]
        public void CompletingWaveEntersRewardPhase()
        {
            var game = new GameManager(cardSeed: 10);
            game.StartWave();

            game.CompleteWave();

            Assert.That(game.State, Is.EqualTo(GameState.Reward));
            Assert.That(game.Waves.State, Is.EqualTo(WaveState.Reward));
        }

        /// <summary>Verifies that a reward draw produces unique choices.</summary>
        [Test]
        public void CardSystemDrawsThreeDistinctPlaceholderCards()
        {
            var cards = new CardSystem(seed: 10).Draw();

            Assert.That(cards, Has.Count.EqualTo(3));
            Assert.That(cards[0].Name, Is.Not.EqualTo(cards[1].Name));
            Assert.That(cards[1].Name, Is.Not.EqualTo(cards[2].Name));
        }

        /// <summary>Verifies that mob cards change shared army values.</summary>
        [Test]
        public void ApplyingMobUpgradeChangesArmyStats()
        {
            var army = new MobArmy();
            float startingDamage = army.Stats.Damage;
            var card = new UpgradeCard("Test", "Test", UpgradeEffect.Damage, 3f, UpgradeTarget.MobArmy, CardRarity.Common);

            army.ApplyUpgrade(card);

            Assert.That(army.Stats.Damage, Is.EqualTo(startingDamage + 3f));
            Assert.That(army.Level, Is.EqualTo(2));
        }

        /// <summary>Verifies the automatic boss growth rule.</summary>
        [Test]
        public void ApplyingBossUpgradeChangesBossStats()
        {
            var boss = new BossWizard();
            float startingDamage = boss.Stats.Damage;

            boss.Upgrade();

            Assert.That(boss.Stats.Damage, Is.GreaterThan(startingDamage));
            Assert.That(boss.Level, Is.EqualTo(2));
        }

        /// <summary>Verifies experience carries across multiple levels.</summary>
        [Test]
        public void ExperienceCanGainMultipleLevels()
        {
            var experience = new Experience(level => 100);

            int levels = experience.Add(250);

            Assert.That(levels, Is.EqualTo(2));
            Assert.That(experience.Level, Is.EqualTo(3));
            Assert.That(experience.Current, Is.EqualTo(50));
        }
    }
}
