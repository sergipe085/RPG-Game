using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [Header("MOVEMENT")]
        [SerializeField] private AudioClip walkClip = null;
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
            navMeshAgent.enabled = !GetComponent<Health>().IsDead();

            UpdateAnimator();
            ExtraRotation();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
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
            if (lookrotation.magnitude > 0.1f && navMeshAgent.velocity.magnitude != 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);
        }

        void FootL()
        {
            if (GetComponent<AudioSource>() == null) { return; }

            GetComponent<AudioSource>().clip = walkClip;
            GetComponent<AudioSource>().Play();
        }

        void FootR()
        {
            if (GetComponent<AudioSource>() == null) { return; }

            GetComponent<AudioSource>().clip = walkClip;
            GetComponent<AudioSource>().Play();
        }
    }
}
