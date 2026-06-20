using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobMentality.Entities
{
    /// <summary>Thin scene adapter that moves and attacks using shared mob stats.</summary>
    public sealed class MobUnit : MonoBehaviour
    {
        private Stats stats;
        private Health health;
        private float attackTimer;

        public event Action<MobUnit> Died;
        public bool IsAlive => health != null && !health.IsDead;

        /// <summary>Initializes this scene unit from the army's current stats.</summary>
        public void Initialize(Stats sourceStats)
        {
            stats = sourceStats;
            health = new Health(stats.MaxHealth);
            attackTimer = 0f;
        }

        /// <summary>Advances basic nearest-enemy movement and combat.</summary>
        public void Tick(IReadOnlyList<EnemyUnit> enemies, float deltaTime)
        {
            if (!IsAlive)
                return;

            EnemyUnit target = FindClosest(enemies);
            if (target == null)
                return;

            attackTimer -= deltaTime;
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance > stats.Range)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, stats.MoveSpeed * deltaTime);
            }
            else if (attackTimer <= 0f)
            {
                target.TakeDamage(stats.Damage);
                attackTimer = 1f / stats.AttackSpeed;
            }
        }

        /// <summary>Applies damage and signals when this unit dies.</summary>
        public void TakeDamage(float amount)
        {
            if (!IsAlive)
                return;

            health.Damage(amount);
            if (health.IsDead)
            {
                Died?.Invoke(this);
                gameObject.SetActive(false);
            }
        }

        private EnemyUnit FindClosest(IReadOnlyList<EnemyUnit> enemies)
        {
            EnemyUnit closest = null;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyUnit candidate = enemies[i];
                if (candidate == null || !candidate.IsAlive)
                    continue;

                float distance = (candidate.transform.position - transform.position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closest = candidate;
                    closestDistance = distance;
                }
            }

            return closest;
        }
    }
}
