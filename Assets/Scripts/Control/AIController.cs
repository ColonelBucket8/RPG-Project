using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        Fighter fighter;
        GameObject player;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");

        }

        private void Update()
        {
            if (DistanceToPlayer() < chaseDistance && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            }
            else
            {
                fighter.Cancel();
            }

        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position);
        }
    }
}