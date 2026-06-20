using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobMentality.Entities
{
    /// <summary>Thin scene adapter for the rival wizard's movement and attacks.</summary>
    public sealed class EnemyUnit : MonoBehaviour
    {
        private Stats stats;
        private Health health;
        private float attackTimer;
        private Action<Vector3, MobUnit, float> castSpell;

        public event Action<EnemyUnit> Died;
        public bool IsAlive => health != null && !health.IsDead;

        /// <summary>Initializes this enemy with wave-scaled stats.</summary>
        public void Initialize(Stats enemyStats)
        {
            stats = enemyStats;
            health = new Health(stats.MaxHealth);
            attackTimer = 0f;
            castSpell = null;
        }

        /// <summary>Initializes the rival wizard with a ranged spell attack.</summary>
        public void InitializeWizard(Stats wizardStats, Action<Vector3, MobUnit, float> spellAction)
        {
            if (spellAction == null)
                throw new ArgumentNullException(nameof(spellAction));

            Initialize(wizardStats);
            castSpell = spellAction;
        }

        /// <summary>Advances basic nearest-mob movement and combat.</summary>
        public void Tick(IReadOnlyList<MobUnit> mobs, float deltaTime)
        {
            if (!IsAlive)
                return;

            MobUnit target = FindClosest(mobs);
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
                if (castSpell == null)
                    target.TakeDamage(stats.Damage);
                else
                    castSpell(transform.position, target, stats.Damage);

                attackTimer = 1f / stats.AttackSpeed;
            }
        }

        /// <summary>Applies damage and signals when this enemy dies.</summary>
        public void TakeDamage(float amount)
        {
            if (!IsAlive)
                return;

            health.Damage(amount);
            if (health.IsDead)
            {
                Died?.Invoke(this);
                Destroy(gameObject);
            }
        }

        private MobUnit FindClosest(IReadOnlyList<MobUnit> mobs)
        {
            MobUnit closest = null;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < mobs.Count; i++)
            {
                MobUnit candidate = mobs[i];
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
