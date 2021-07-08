using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target = null;
        [SerializeField] private float sensitivity = 0f;
        float xRotation;

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                Rotate();
            }
        }

        void Rotate()
        {
            xRotation += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
            transform.rotation = Quaternion.Euler(transform.rotation.y, xRotation, transform.rotation.z);
        }

        void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 8f);
        }
    }
}
