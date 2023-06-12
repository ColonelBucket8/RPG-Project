using UnityEngine;

namespace RPG.Stats
{

    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        Experience experience;
        int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();

            experience = GetComponent<Experience>();

            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                print("Levelled up!");
            }
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }


        public int CalculateLevel()
        {
            experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentXP = GetComponent<Experience>().GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level < penultimateLevel; level++)
            {
                float xPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (xPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

    }
}
