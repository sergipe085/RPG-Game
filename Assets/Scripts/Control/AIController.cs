using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance;

        Fighter fighter;
        GameObject playerRef;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            playerRef = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (InAttackRangeOfPlayer() && fighter.CanAttack(playerRef))
            {
                fighter.Attack(playerRef);
            }
            else
            {
                fighter.Cancel();
            }
        }

        bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(transform.position, playerRef.transform.position) <= chaseDistance;
        }
    }
}
