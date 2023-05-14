using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        SavingSystem savingSystem;

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
        }

        private void Start()
        {
            Load();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Delete();
            }
        }

        private void Delete()
        {
            savingSystem.Delete(defaultSaveFile);
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }
    }
}