using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        Vector3 initialPosition;
        Quaternion initialRotation;

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        private void Update()
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }

        void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if (i == 0)
                    Gizmos.color = Color.green;
                else if (i == transform.childCount - 1)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.white;

                Gizmos.DrawSphere(GetWayPoint(i), 0.2f);

                Gizmos.color = Color.white;

                int j = GetNextIndex(i);

                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 >= transform.childCount) { return 0; }
            return i + 1;
        }


        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
