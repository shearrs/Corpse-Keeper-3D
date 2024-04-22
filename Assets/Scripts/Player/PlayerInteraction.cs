using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    [System.Serializable]
    public class PlayerInteraction
    {
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private float interactionRange = 3;
        [SerializeField] private float castRadius = 0.5f;
        [SerializeField] private LayerMask interactMask;

        private Transform Transform 
        { 
            get 
            {
                if (_transform == null)
                    _transform = Camera.main.transform;

                return _transform;
            } 
        }
        private Transform _transform;

        public void CheckInteraction()
        {
            Physics.SphereCast(Transform.position, castRadius, Transform.forward, out RaycastHit hit, interactionRange, interactMask);

            if (hit.transform == null)
                return;

            hit.transform.TryGetComponent(out IInteractable interactable);

            if (interactable == null)
                return;

            interactable.Interact();
        }

        public void DrawGizmos()
        {
            if (!drawGizmos)
                return;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(Transform.position, Transform.forward * interactionRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Transform.position + Transform.forward, castRadius);
        }
    }
}