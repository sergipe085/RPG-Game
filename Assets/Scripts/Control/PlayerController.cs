using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Resources;
using System;
using UnityEngine.EventSystems;

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

        [SerializeField] private LayerMask movementLayers = 0;

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
            RaycastHit hit;
            if (Physics.Raycast(GetMouseRay(), out hit, Mathf.Infinity, movementLayers))
            {
                if (Input.GetMouseButton(0))
                {
                    mover.currentSpeed = fighter.currentWeapon.playerSpeed;
                    mover.StartMoveAction(hit.point);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
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
