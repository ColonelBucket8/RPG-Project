using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        SavingSystem savingSystem;

        const string defaultSaveFile = "save";

        private void Start()
        {
            savingSystem = GetComponent<SavingSystem>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        private void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }

        private void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }
    }
}