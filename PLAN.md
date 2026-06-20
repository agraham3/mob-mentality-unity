# Development plan

The roadmap favors one reviewable gameplay improvement at a time.

## 1. Foundation — complete

- Unity 6 2D project layout and `Main` scene
- Plain C# models with thin Unity scene adapters
- Core events, documentation, and EditMode tests

## 2. Core wave loop — first slice complete

- Spawn scaled placeholder enemies
- Run basic automatic movement and combat
- Complete a wave when all enemies are defeated
- Next: add an explicit defeat/retry path when the whole mob dies

## 3. Card and upgrade system — first slice complete

- Draw three unique placeholder cards
- Apply health, damage, speed, attack speed, or range upgrades
- Next: move authored card definitions into small ScriptableObject assets

## 4. Mob army and boss progression — first slice complete

- Share army stats across spawned units
- Automatically improve the boss after each player choice
- Next: give the boss a simple milestone encounter and telegraphed attack

## 5. Basic UI — first slice complete

- Show wave, phase, mob strength, boss strength, controls, and rewards
- Next: show individual health bars and a concise wave progress counter

## 6. Playable vertical slice — in progress

- Current: repeatable wave → combat → reward → upgrades → next wave loop
- Next: add defeat/restart, a short boss milestone, and one meaningful enemy variant

## 7. Testing and polish — ongoing

- Expand EditMode tests as rules are added
- Add focused PlayMode tests for scene wiring and automatic wave completion
- Add readability, timing, audio, and VFX only after the loop is stable
