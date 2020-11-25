using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        float xRotation, yRotation;

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                Rotate();
            }
        }

        void Rotate()
        {
            xRotation += Input.GetAxis("Mouse X");
            transform.rotation = Quaternion.Euler(transform.rotation.y, xRotation, transform.rotation.z);
        }

        void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 8f);
        }
    }
}
