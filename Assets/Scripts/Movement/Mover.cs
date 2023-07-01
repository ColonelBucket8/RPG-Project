using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField]
        Transform target;
        [SerializeField]
        float maxSpeed = 6f;
        [SerializeField]
        float maxNavPathLength = 40f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            // Enable to pass through dead body
            navMeshAgent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;

            // Animator is only interested in z velocity, moving forward
            // The global value is not useful for the animator
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void Cancel() { navMeshAgent.isStopped = true; }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination,
                                                 NavMesh.AllAreas, path);

            if (!hasPath)
                return false;

            if (path.status != NavMeshPathStatus.PathComplete)
                return false;

            if (GetPathLength(path) > maxNavPathLength)
                return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            Vector3[] corners = path.corners;

            float sum = 0f;

            if (corners.Length < 2)
                return sum;

            for (int i = 0; i < corners.Length - 1; i++)
            {
                float distance = Vector3.Distance(corners[i], corners[i + 1]);
                sum += distance;
            }

            return sum;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        /// <summary>
        /// Cancel other action and move to the pointed destination
        /// </summary>
        /// <param name="destination"></param>
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;

            // Stop NavMeshAgent interference
            navMeshAgent.enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            navMeshAgent.enabled = true;
        }
    }
}
