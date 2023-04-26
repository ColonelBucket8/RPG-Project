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
                int j = i == transform.childCount - 1 ? 0 : i + 1;
                Gizmos.DrawSphere(GetWaypoint(i), sphereRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));

            }

        }

        private Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}