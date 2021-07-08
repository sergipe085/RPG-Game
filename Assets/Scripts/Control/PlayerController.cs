using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping {
            public CursorType cursorType;
            public Texture2D  texture;
            public Vector2    hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        [SerializeField] private LayerMask movementLayers   = 0;
        [SerializeField] private float     maxNavPathLength = 10;

        [Header("Components")]
        public  Fighter fighter = null;
        private Mover   mover   = null;

        private void Awake() {
            fighter = GetComponent<Fighter>();
            mover   = GetComponent<Mover>();
        }

        private void Update()
        {
            if (InteractWithUI()) { return; };
            if (GetComponent<Health>().IsDead()) { SetCursor(CursorType.None); return; }
            if (InteractWithComponents()) { return; }
            if (InteractWithMovement()) { return; }

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponents() {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits) {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables) {
                    if (raycastable.HandleRaycast(this)) {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastAllSorted() {
            RaycastHit[] hits      = Physics.RaycastAll(GetMouseRay());
            float[]      distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++) {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithMovement()
        {
            if (RaycastNavMesh(out Vector3 target))
            {
                if (Input.GetMouseButton(0))
                {
                    mover.currentSpeed = fighter.currentWeaponConfig.playerSpeed;
                    mover.StartMoveAction(target);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            SetCursor(CursorType.None);
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target) {
            target = new Vector3();
            if (!Physics.Raycast(GetMouseRay(), out RaycastHit hit, Mathf.Infinity, movementLayers)) return false;

            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, Mathf.Infinity, NavMesh.AllAreas)) {
                target = navMeshHit.position;

                NavMeshPath path = new NavMeshPath();
                if (!NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path)) { return false; }
                if (path.status != NavMeshPathStatus.PathComplete) { return false; }
                if (GetPathLength(path) > maxNavPathLength) { return false; }

                return true;
            }
            return false;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float length = 0.0f;

            if (path.corners.Length < 2) return length;

            for (int i = 0; i < path.corners.Length - 1; i++) {
                float dis = Vector3.Distance(path.corners[i], path.corners[i + 1]);
                length += dis;
            }

            return length;
        }

        private bool InteractWithUI() {
            if (EventSystem.current.IsPointerOverGameObject()) {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }


        private void SetCursor(CursorType type) {
            CursorMapping? mapping = GetCursorMapping(type);
            if (mapping != null) {
                Cursor.SetCursor(mapping.Value.texture, mapping.Value.hotspot, CursorMode.Auto);
            }
        }

        private CursorMapping? GetCursorMapping(CursorType cursorType) {
            foreach(CursorMapping map in cursorMappings) {
                if (map.cursorType == cursorType) {
                    return map;
                }
            }
            return null;
        }
    }
}
