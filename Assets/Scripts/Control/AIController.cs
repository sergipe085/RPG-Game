﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float patrolSpeed = 0f;
        [SerializeField] private float attackSpeed = 0f;
        [SerializeField] private float chaseDistance;
        [SerializeField] private float suspicionTime;
        [SerializeField] private float guardTime = 1f;
        [SerializeField] private float waypointTolerance = 1f;

        PatrolPath patrolPath;
        Fighter fighter;
        Health health;
        GameObject playerRef;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceLastWaypoint = 0f;
        int waypointIndex = 0;

        private void Start()
        {
            patrolPath = GetComponentInChildren<PatrolPath>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
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

            GetComponent<Mover>().StartMoveAction(nextPosition);
            GetComponent<NavMeshAgent>().speed = patrolSpeed;
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
            GetComponent<NavMeshAgent>().speed = attackSpeed;
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