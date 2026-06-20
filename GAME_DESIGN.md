# Game design

## Concept

`mob-mentality` is a 2D roguelike / wave-survival game about cultivating an unruly little army. The player is the army and pursues a single rival wizard. Choices improve the mob as a group while the wizard grows after every reward.

## Main loop

1. Start a wave.
2. The rival wizard enters the arena.
3. The mob army pursues and attacks the wizard while he casts spells at them.
4. Defeating the wizard completes the wave.
5. Three upgrade cards appear.
6. The player chooses one mob upgrade.
7. The boss wizard automatically upgrades.
8. The player starts the next wave.

## Entities

- **Mob army:** the player-controlled side. It owns shared progression and spawns three blue units per wave.
- **Mob unit:** a simple scene representation that seeks and attacks the nearest living enemy.
- **Boss wizard:** the army's single purple target. He launches visible spells at the nearest living mob and grows after each reward.

## Upgrade examples

- **Thick Skins:** increase maximum health.
- **Sharp Sticks:** increase damage.
- **Battle Rhythm:** increase attack speed.
- **Quick Feet:** increase movement speed.
- **Long Reach:** increase attack range.

Rarity is displayed now but does not yet affect draw weighting. A future uncommon or rare boss upgrade may allow the wizard to clone himself.

## Progression

Every chosen card raises the mob army by one level and changes one shared stat. Newly spawned mobs use those improved stats. After the choice, the boss gains maximum health, damage, and one level. HUD strength is a simple comparison score derived from health, damage, and attack speed; it is not a separate combat stat.

## Early vertical slice scope

The slice proves that the complete loop is readable and repeatable using placeholder visuals. It includes simple combat and reward decisions, but intentionally excludes final art, audio, VFX, saves, inventory, procedural maps, networking, complex AI, and the final boss encounter.
