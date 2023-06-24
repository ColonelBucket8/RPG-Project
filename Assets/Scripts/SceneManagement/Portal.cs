using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {

        enum DestinationIdentifier
        {
            A,
            B,
            C,
            D,
            E
        }

        [SerializeField]
        int sceneToLoad = -1;
        [SerializeField]
        Transform spawnPoint;
        [SerializeField]
        DestinationIdentifier destination;
        [SerializeField]
        private float timeToFadeOut = 3f;
        [SerializeField]
        private float timeToFadeIn = 1f;
        [SerializeField]
        private float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            PlayerController playerController =
                GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(timeToFadeOut);

            // Save current level
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // Getting new player tag after loading new scene
            PlayerController newPlayerController =
                GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            // Load current level
            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal(destination);
            UpdatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(timeToFadeIn);

            newPlayerController.enabled = true;

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal(DestinationIdentifier destination)
        {
            Portal[] portals = FindObjectsOfType<Portal>();

            foreach (Portal portal in portals)
            {
                if (portal != this && portal.destination == destination)
                {
                    return portal;
                }
            }

            return null;
        }
    }
}
