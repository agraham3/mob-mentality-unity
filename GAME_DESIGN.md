# Game design

## Concept

`mob-mentality` is a 2D roguelike / wave-survival game about cultivating an unruly little army. The player does not gain direct combat power; choices improve the mob as a group. A rival wizard grows after every reward, creating an escalating race toward a later confrontation.

## Main loop

1. Start a wave.
2. Enemies spawn with wave-scaled health and damage.
3. The mob army automatically moves, attacks, and survives.
4. Defeating all enemies completes the wave.
5. Three upgrade cards appear.
6. The player chooses one mob upgrade.
7. The boss wizard automatically upgrades.
8. The player starts the next wave.

## Entities

- **Mob army:** owns shared progression and stats. The current placeholder army spawns three blue units per wave.
- **Mob unit:** a simple scene representation that seeks and attacks the nearest living enemy.
- **Enemy unit:** a red placeholder that seeks and attacks the nearest living mob.
- **Boss wizard:** a visible purple placeholder and progression model. It does not fight in this first slice.

## Upgrade examples

- **Thick Skins:** increase maximum health.
- **Sharp Sticks:** increase damage.
- **Battle Rhythm:** increase attack speed.
- **Quick Feet:** increase movement speed.
- **Long Reach:** increase attack range.

Rarity is displayed now but does not yet affect draw weighting.

## Progression

Every chosen card raises the mob army by one level and changes one shared stat. Newly spawned mobs use those improved stats. After the choice, the boss gains maximum health, damage, and one level. HUD strength is a simple comparison score derived from health, damage, and attack speed; it is not a separate combat stat.

## Early vertical slice scope

The slice proves that the complete loop is readable and repeatable using placeholder visuals. It includes simple combat and reward decisions, but intentionally excludes final art, audio, VFX, saves, inventory, procedural maps, networking, complex AI, and the final boss encounter.
