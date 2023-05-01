using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        PlayableDirector playableDirector;

        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            playableDirector.played += DisableControl;
            playableDirector.stopped += EnableControl;
        }

        void DisableControl(PlayableDirector pd)
        {
            print("Disable control");

        }

        void EnableControl(PlayableDirector pd)
        {
            print("Enable control");
        }

    }
}