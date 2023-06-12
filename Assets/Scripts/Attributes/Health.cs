using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 20f;

        BaseStats baseStats;
        bool isDead = false;

        CapsuleCollider capsuleCollider;

        private void Start()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            baseStats = GetComponent<BaseStats>();
            healthPoints = baseStats.GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            // Avoid reducing health below than zero
            // If health minus damage is less than zero
            // take zero and assign it to health
            healthPoints = Mathf.Max(healthPoints - damage, 0);


            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }

        }

        public float GetPercentage()
        {
            return 100 * (healthPoints / baseStats.GetStat(Stat.Health));
        }

        private void Die()
        {
            if (isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();

            isDead = true;

            // Player doesn't have capsule collider component
            // Capsule collider is attached to enemy for ray cast
            if (capsuleCollider != null)
            {
                capsuleCollider.enabled = false;
            }

        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();

            if (experience)
            {
                float xp = baseStats.GetStat(Stat.ExperienceReward);
                experience.GainExperience(xp);
            }
        }

        public object CaptureState()
        {
            // Primitive values don't need to serialize
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}
