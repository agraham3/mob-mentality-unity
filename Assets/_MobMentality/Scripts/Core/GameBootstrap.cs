using System.Collections.Generic;
using MobMentality.Entities;
using MobMentality.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MobMentality.Core
{
    /// <summary>Builds and drives the placeholder scene around the testable game model.</summary>
    public sealed class GameBootstrap : MonoBehaviour
    {
        private readonly List<MobUnit> mobs = new List<MobUnit>();
        private readonly List<EnemyUnit> enemies = new List<EnemyUnit>();
        private GameManager game;
        private HudController hud;
        private RewardPhaseController rewards;
        private Button startButton;
        private Button completeButton;
        private Transform mobRoot;
        private Transform enemyRoot;
        private Transform spellRoot;
        private Sprite squareSprite;

        private void Start()
        {
            game = new GameManager();
            CreateSystemMarkers();
            CreateArena();
            CreateInterface();
            RefreshInterface();
        }

        private void Update()
        {
            if (game == null)
                return;

            if (game.State == GameState.Wave)
                TickCombat(Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space) && game.State == GameState.Ready)
                StartWave();
            if (Input.GetKeyDown(KeyCode.C) && game.State == GameState.Wave)
                FinishWave();
        }

        private void CreateSystemMarkers()
        {
            new GameObject("WaveManager").transform.SetParent(transform);
            new GameObject("CardSystem").transform.SetParent(transform);
            mobRoot = new GameObject("MobArmy").transform;
            mobRoot.SetParent(transform);
            enemyRoot = new GameObject("Enemies").transform;
            enemyRoot.SetParent(transform);
            spellRoot = new GameObject("Spells").transform;
            spellRoot.SetParent(transform);
        }

        private void CreateArena()
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                camera.orthographic = true;
                camera.orthographicSize = 5.5f;
                camera.backgroundColor = new Color(0.055f, 0.07f, 0.09f);
            }

            squareSprite = CreateSquareSprite();
        }

        private void CreateInterface()
        {
            Canvas canvas = RuntimeUiFactory.CreateCanvas();

            Text hudText = RuntimeUiFactory.CreateText(canvas.transform, "Status", 22, TextAnchor.UpperLeft);
            RuntimeUiFactory.SetRect(hudText.rectTransform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(24f, -170f), new Vector2(500f, -20f));
            hud = canvas.gameObject.AddComponent<HudController>();
            hud.Initialize(hudText);

            startButton = RuntimeUiFactory.CreateButton(canvas.transform, "StartWaveButton", "Start Wave  [Space]");
            RuntimeUiFactory.SetRect((RectTransform)startButton.transform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(24f, 24f), new Vector2(240f, 82f));
            startButton.onClick.AddListener(StartWave);

            completeButton = RuntimeUiFactory.CreateButton(canvas.transform, "CompleteWaveButton", "Debug: Complete  [C]");
            RuntimeUiFactory.SetRect((RectTransform)completeButton.transform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(252f, 24f), new Vector2(480f, 82f));
            completeButton.onClick.AddListener(FinishWave);

            GameObject rewardPanel = RuntimeUiFactory.CreatePanel(canvas.transform, "RewardPanel", new Color(0.06f, 0.08f, 0.12f, 0.97f));
            RuntimeUiFactory.SetRect((RectTransform)rewardPanel.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-390f, -155f), new Vector2(390f, 155f));
            Text title = RuntimeUiFactory.CreateText(rewardPanel.transform, "Title", 28, TextAnchor.MiddleCenter);
            RuntimeUiFactory.SetRect(title.rectTransform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(12f, -62f), new Vector2(-12f, -10f));

            var cardButtons = new Button[3];
            for (int i = 0; i < cardButtons.Length; i++)
            {
                cardButtons[i] = RuntimeUiFactory.CreateButton(rewardPanel.transform, $"Card{i + 1}", "Card");
                float left = 18f + i * 252f;
                RuntimeUiFactory.SetRect((RectTransform)cardButtons[i].transform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(left, 18f), new Vector2(left + 238f, 235f));
            }

            rewards = canvas.gameObject.AddComponent<RewardPhaseController>();
            rewards.Initialize(rewardPanel, title, cardButtons);
        }

        private void StartWave()
        {
            if (game.State != GameState.Ready)
                return;

            game.StartWave();
            SpawnMobs();
            SpawnWizard();
            RefreshInterface();
        }

        private void SpawnMobs()
        {
            ClearChildren(mobRoot);
            ClearChildren(spellRoot);
            mobs.Clear();
            for (int i = 0; i < 3; i++)
            {
                GameObject unitObject = CreateSquare($"Mob {i + 1}", new Vector2(-4.5f, -1.2f + i * 1.2f), new Color(0.2f, 0.65f, 1f), 0.72f);
                unitObject.transform.SetParent(mobRoot);
                MobUnit unit = unitObject.AddComponent<MobUnit>();
                unit.Initialize(game.MobArmy.Stats);
                unit.Died += OnMobDied;
                mobs.Add(unit);
            }
        }

        private void CastSpell(Vector3 origin, MobUnit target, float damage)
        {
            GameObject spellObject = CreateSquare("Wizard Spell", origin, new Color(0.85f, 0.4f, 1f), 0.22f);
            spellObject.transform.SetParent(spellRoot);
            SpellProjectile spell = spellObject.AddComponent<SpellProjectile>();
            spell.Initialize(target, damage, 7f);
        }

        private void SpawnWizard()
        {
            ClearChildren(enemyRoot);
            enemies.Clear();
            GameObject wizardObject = CreateSquare("Boss Wizard", new Vector2(4f, 0f), new Color(0.65f, 0.25f, 0.95f), 1.1f);
            wizardObject.transform.SetParent(enemyRoot);
            EnemyUnit wizard = wizardObject.AddComponent<EnemyUnit>();
            wizard.InitializeWizard(game.BossWizard.Stats.Clone(), CastSpell);
            wizard.Died += OnEnemyDied;
            enemies.Add(wizard);
        }

        private void TickCombat(float deltaTime)
        {
            for (int i = mobs.Count - 1; i >= 0; i--)
                mobs[i].Tick(enemies, deltaTime);
            for (int i = enemies.Count - 1; i >= 0; i--)
                enemies[i].Tick(mobs, deltaTime);
        }

        private void OnEnemyDied(EnemyUnit enemy)
        {
            enemy.Died -= OnEnemyDied;
            enemies.Remove(enemy);
            if (enemies.Count == 0 && game.State == GameState.Wave)
                FinishWave();
        }

        private void OnMobDied(MobUnit mob)
        {
            mob.Died -= OnMobDied;
            mobs.Remove(mob);
            if (mobs.Count == 0 && game.State == GameState.Wave)
            {
                game.DefeatArmy();
                ClearChildren(spellRoot);
                rewards.ShowGameOver();
                RefreshInterface();
            }
        }

        private void FinishWave()
        {
            if (game.State != GameState.Wave)
                return;

            var cards = game.CompleteWave();
            rewards.Show(cards, card =>
            {
                game.ChooseCard(card);
                rewards.Hide();
                RefreshInterface();
            });
            RefreshInterface();
        }

        private void RefreshInterface()
        {
            hud.Refresh(game);
            startButton.gameObject.SetActive(game.State == GameState.Ready);
            startButton.GetComponentInChildren<Text>().text = game.Waves.CurrentWave == 1 ? "Start Wave  [Space]" : "Start Next Wave  [Space]";
            completeButton.gameObject.SetActive(game.State == GameState.Wave);
        }

        private GameObject CreateSquare(string objectName, Vector2 position, Color color, float size)
        {
            var gameObject = new GameObject(objectName, typeof(SpriteRenderer));
            gameObject.transform.position = position;
            gameObject.transform.localScale = Vector3.one * size;
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.sprite = squareSprite != null ? squareSprite : CreateSquareSprite();
            renderer.color = color;
            return gameObject;
        }

        private static Sprite CreateSquareSprite()
        {
            var texture = new Texture2D(1, 1) { name = "Placeholder Square" };
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), Vector2.one * 0.5f, 1f);
        }

        private static void ClearChildren(Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
                Destroy(root.GetChild(i).gameObject);
        }
    }
}
