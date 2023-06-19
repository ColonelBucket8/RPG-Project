using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField]
        float regenerationPercentage = 70f;
        [SerializeField]
        UnityEvent<float> takeDamage;

        LazyValue<float> healthPoints;
        BaseStats baseStats;
        CapsuleCollider capsuleCollider;
        bool isDead = false;

        private void Awake()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            baseStats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth() { return baseStats.GetStat(Stat.Health); }

        private void Start() { healthPoints.ForceInit(); }

        private void OnEnable() { baseStats.onLevelUp += RegenerateHealth; }

        private void OnDisable() { baseStats.onLevelUp -= RegenerateHealth; }

        public bool IsDead() { return isDead; }

        public void TakeDamage(GameObject instigator, float damage)
        {
            // Avoid reducing health below than zero
            // If health minus damage is less than zero
            // take zero and assign it to health
            print(gameObject.name + " took damage: " + damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if (healthPoints.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }

            takeDamage.Invoke(damage);
        }

        public float GetHealthPoints() { return healthPoints.value; }

        public float GetMaxHealthPoints() { return baseStats.GetStat(Stat.Health); }

        public float GetPercentage()
        {
            return 100 * (healthPoints.value / baseStats.GetStat(Stat.Health));
        }

        public void RegenerateHealth()
        {
            float regenHealthPoints =
                baseStats.GetStat(Stat.Health) * (regenerationPercentage / 100);

            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        private void Die()
        {
            if (isDead)
                return;

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
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value == 0)
            {
                Die();
            }
        }
    }
}
