using System.Collections.Generic;
using MobMentality.Cards;
using MobMentality.Core;
using MobMentality.Entities;
using MobMentality.Waves;
using NUnit.Framework;
using UnityEngine;

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
            Assert.That(definition.EnemyCount, Is.EqualTo(1));
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
            Assert.That(game.LastRoundOutcome, Is.EqualTo(RoundOutcome.ArmyVictory));
        }

        /// <summary>Verifies that losing the army ends the run with a wizard victory.</summary>
        [Test]
        public void DefeatingArmyEndsGame()
        {
            var game = new GameManager();
            game.StartWave();

            game.DefeatArmy();

            Assert.That(game.State, Is.EqualTo(GameState.GameOver));
            Assert.That(game.Waves.State, Is.EqualTo(WaveState.Defeated));
            Assert.That(game.LastRoundOutcome, Is.EqualTo(RoundOutcome.WizardVictory));
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

        /// <summary>Verifies that the rival wizard delegates its attack to the spell action.</summary>
        [Test]
        public void RivalWizardCastsSpellAtMobInRange()
        {
            var wizardObject = new GameObject("Test Rival Wizard");
            var mobObject = new GameObject("Test Mob");

            try
            {
                EnemyUnit wizard = wizardObject.AddComponent<EnemyUnit>();
                MobUnit mob = mobObject.AddComponent<MobUnit>();
                mobObject.transform.position = Vector3.right;
                mob.Initialize(new Stats(10f, 1f, 1f, 1f, 1f));
                int spellsCast = 0;
                wizard.InitializeWizard(new Stats(10f, 2f, 1f, 1f, 3f), (origin, target, damage) => spellsCast++);

                wizard.Tick(new List<MobUnit> { mob }, 0f);

                Assert.That(spellsCast, Is.EqualTo(1));
            }
            finally
            {
                Object.DestroyImmediate(wizardObject);
                Object.DestroyImmediate(mobObject);
            }
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
