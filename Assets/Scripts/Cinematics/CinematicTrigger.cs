using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {

        [SerializeField] bool isCinematicDisabled = false;
        bool hasTriggered = false;

        public object CaptureState()
        {
            return hasTriggered;
        }

        public void RestoreState(object state)
        {
            hasTriggered = (bool)state;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered || isCinematicDisabled) return;

            if (other.CompareTag("Player"))
            {
                hasTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }

    }
}
