using System;
using UnityEngine;

namespace RPG.Stats
{

    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField]
        int startingLevel = 1;
        [SerializeField]
        CharacterClass characterClass;
        [SerializeField]
        Progression progression = null;
        [SerializeField]
        GameObject levelUpParticleEffect = null;
        [SerializeField]
        bool shouldUseModifiers = false;

        Experience experience;
        int currentLevel = 0;

        public event Action onLevelUp;

        private void Awake() { experience = GetComponent<Experience>(); }

        private void Start() { currentLevel = CalculateLevel(); }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAddictiveModifier(stat)) *
                   (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAddictiveModifier(Stat stat)
        {
            if (!shouldUseModifiers)
                return 0;

            float sum = 0f;
            foreach (IModifierProvider provider in
                         GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAddictiveModifiers(stat))
                {
                    sum += modifier;
                }
            }

            return sum;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers)
                return 0;

            float sum = 0f;
            foreach (IModifierProvider provider in
                         GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    sum += modifier;
                }
            }

            return sum;
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }

        private int CalculateLevel()
        {
            experience = GetComponent<Experience>();

            if (experience == null)
                return startingLevel;

            float currentXP = GetComponent<Experience>().GetPoints();
            int penultimateLevel =
                progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level < penultimateLevel; level++)
            {
                float xPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp,
                                                        characterClass, level);
                if (xPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}
