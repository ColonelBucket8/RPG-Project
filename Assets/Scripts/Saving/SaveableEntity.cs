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
            print("Capturing state for: " + GetUniqueIdentifier());
            return null;
        }

        public void RestoreState(object state)
        {
            print("Capturing state for: " + GetUniqueIdentifier());
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