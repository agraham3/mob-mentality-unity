using UnityEngine;

namespace MobMentality.Entities
{
    /// <summary>Moves a wizard spell toward its target and applies damage on impact.</summary>
    public sealed class SpellProjectile : MonoBehaviour
    {
        private const float ImpactDistance = 0.12f;
        private MobUnit target;
        private float damage;
        private float speed;

        /// <summary>Sets the target and combat values for this spell.</summary>
        public void Initialize(MobUnit spellTarget, float spellDamage, float moveSpeed)
        {
            target = spellTarget;
            damage = spellDamage;
            speed = moveSpeed;
        }

        private void Update()
        {
            if (target == null || !target.IsAlive)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, target.transform.position) > ImpactDistance)
                return;

            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
