using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float sphereRadius = 0.3f;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < transform.childCount; i++)
            {
                // If last element, draw from the last element to the first element
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), sphereRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));

            }

        }

        public int GetNextIndex(int i)
        {
            if (i == transform.childCount - 1)
            {
                return 0;
            }

            return i + 1;

        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}