using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 50;

        [Header("Limits")]
        [SerializeField, Range(-89f, 89f)] private float minVerticalRotation = -50;
        [SerializeField, Range(-89f, 89f)] private float maxVerticalRotation = 80;

        private Vector2 orbitAngles = Vector2.zero;

        private void Start()
        {
            MenuCamera.Instance.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnValidate()
        {
            if (maxVerticalRotation < minVerticalRotation)
                maxVerticalRotation = minVerticalRotation;
        }

        private void Update()
        {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            Vector2 movementInput = PlayerInputHandler.CameraMovementInput;
            Vector2 input = new(-movementInput.y, movementInput.x);
            orbitAngles += sensitivity * Time.deltaTime * input;

            ConstrainAngles();

            Vector3 lookDirection = Quaternion.Euler(orbitAngles) * Vector3.forward;

            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }

        private void ConstrainAngles()
        {
            orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalRotation, maxVerticalRotation);

            if (orbitAngles.y < 0f)
                orbitAngles.y += 360f;
            else if (orbitAngles.y >= 360f)
                orbitAngles.y -= 360f;
        }
    }
}