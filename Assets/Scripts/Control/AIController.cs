using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float patrolSpeed = 0.0f;
        [SerializeField] private float chaseSpeed = 0.0f;
        [SerializeField] private float chaseDistance = 0.0f;
        [SerializeField] private float suspicionTime;
        [SerializeField] private float guardTime = 1f;
        [SerializeField] private float waypointTolerance = 1f;

        [SerializeField] private PatrolPath patrolPath;
        Fighter fighter;
        Health health;
        Mover mover;
        GameObject playerRef;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceLastWaypoint = 0f;
        int waypointIndex = 0;

        private void Awake() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) { return; }

            if (InAttackRangeOfPlayer() && fighter.CanAttack(playerRef))
            {
                AttackBehaviour();
                timeSinceLastSawPlayer = 0;
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
                timeSinceLastSawPlayer += Time.deltaTime;
            }
            else
            {
                PatrolBehaviour();
            }
        }

        void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            mover.StartMoveAction(nextPosition);
            mover.currentSpeed = patrolSpeed;
        }

        bool AtWayPoint()
        {
            float distanceToWaypont = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypont <= waypointTolerance;
        }

        Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPoint(waypointIndex);
        }

        void CycleWaypoint()
        {
            timeSinceLastWaypoint += Time.deltaTime;
            if (timeSinceLastWaypoint > guardTime)
            {
                waypointIndex = patrolPath.GetNextIndex(waypointIndex);
                timeSinceLastWaypoint = 0f;
            }
        }

        void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        void AttackBehaviour()
        {
            fighter.Attack(playerRef);
            mover.currentSpeed = chaseSpeed;
        }

        bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(transform.position, playerRef.transform.position) <= chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
