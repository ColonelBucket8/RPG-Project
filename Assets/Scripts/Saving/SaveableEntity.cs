using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            ISaveable[] collection = GetComponents<ISaveable>();

            foreach (ISaveable saveable in collection)
            {
                string typeString = saveable.GetType().ToString();
                state[typeString] = saveable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

            ISaveable[] collection = GetComponents<ISaveable>();

            foreach (ISaveable saveable in collection)
            {
                string typeString = saveable.GetType().ToString();

                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }

        }

        // Remove update method from build
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;

            // Empty scene path means the editor is in prefab
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            // Only assign uuid to instance of prefab 
            // To avoid uuid duplication
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue))
            {
                uniqueIdentifier = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}