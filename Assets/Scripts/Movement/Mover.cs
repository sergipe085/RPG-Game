using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [Header("MOVEMENT")]
        [SerializeField] private float extraRotationSpeed = 0.0f;

        [Header("COMPONENTS")]
        NavMeshAgent navMeshAgent;
        Animator anim;

        void Start()
        {
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateAnimator();
            ExtraRotation();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }

        public void Stop()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            anim.SetFloat("ForwardSpeed", localVelocity.z);
        }

        void ExtraRotation()
        {
            Vector3 lookrotation = navMeshAgent.steeringTarget - transform.position;
            if (lookrotation.magnitude > 0.1f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);
        }
    }
}
