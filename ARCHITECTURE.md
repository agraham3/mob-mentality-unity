# Architecture

## Folder structure

```text
Assets/_MobMentality/
├── Scenes/                 Main playable scene
├── Scripts/
│   ├── Core/               Run state, events, orchestration, scene bootstrap
│   ├── Waves/              Wave rules and definitions
│   ├── Cards/              Card definitions, draw logic, upgrade enums
│   ├── Entities/           Stats, progression models, combat adapters
│   ├── UI/                 HUD, reward panel, runtime UI construction
│   └── Data/               Reserved for future authored data types
├── Prefabs/                Reserved for future reusable scene objects
├── ScriptableObjects/      Reserved for future authored data assets
└── Tests/
    ├── EditMode/           Fast rule tests
    └── PlayMode/           Reserved for scene integration tests
```

## Core systems

`GameManager` coordinates `WaveManager`, `CardSystem`, `MobArmy`, and `BossWizard`. It owns the allowed run transitions but contains no rendering or input code. `GameEvents` exposes the important wave, reward, card, mob, boss, and game-state notifications.

`WaveManager` validates wave transitions and creates a small scaled `WaveDefinition`. `CardSystem` draws unique cards from the starter deck. `Stats`, `Health`, and `Experience` are independent rule objects.

`GameBootstrap` is the Unity adapter for this intentionally asset-light slice. On play it creates named system markers, placeholder sprites, controls, and UI around the plain C# game model. `MobUnit`, `EnemyUnit`, and `SpellProjectile` are MonoBehaviours because movement and visual lifetime require transforms and GameObjects.

## Plain C# versus MonoBehaviour

Keep deterministic state and rules in plain C#: game/wave state, card selection, stat upgrades, health, experience, mob progression, and boss progression. This makes them fast to instantiate and test without loading a scene.

Use MonoBehaviours only where Unity is the point: scene startup, frame ticks, input, transforms, sprite lifetime, buttons, and text. Avoid putting upgrade rules or wave transition validation in UI and unit components.

## Testing approach

EditMode tests cover state transitions, card draws, mob upgrades, boss upgrades, and experience leveling. They compile against `MobMentality.Runtime` through an assembly definition. Future PlayMode tests should remain narrow: load `Main`, verify the runtime objects exist, and prove that eliminating the final enemy opens rewards. Avoid timing-heavy tests when the same rule can be covered in EditMode.
