using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBobbing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform bone;
    private CustomController controller;

    [Header("Settings")]
    [SerializeField] private float bobbingHeight = 1;
    [SerializeField] private float bobbingSpeed = 1;

    private Coroutine bobCoroutine;
    private Coroutine smoothCoroutine;

    private bool PlayerIsMoving => controller.IsGrounded && controller.Velocity.sqrMagnitude > 1;

    private void Awake()
    {
        controller = GetComponent<CustomController>();
    }

    public void Update()
    {
        if (bone != null && bobCoroutine == null && PlayerIsMoving)
            bobCoroutine = StartCoroutine(IEBob());
    }

    private IEnumerator IEBob()
    {
        if (smoothCoroutine != null)
            StopCoroutine(smoothCoroutine);

        float t = 0;
        float speed = bobbingSpeed;
        Vector3 originalPosition = bone.localPosition;

        while (PlayerIsMoving)
        {
            if (PlayerInputHandler.SprintInput)
                speed = bobbingSpeed * 1.5f;
            else
                speed = bobbingSpeed;

            Vector3 position = bone.localPosition;
            position.y += (0.01f * bobbingHeight) * Mathf.Sin(speed * t);

            bone.localPosition = position;

            t += Time.deltaTime;

            yield return null;
        }

        bobCoroutine = null;

        smoothCoroutine = StartCoroutine(IEReturn(originalPosition));
    }

    private IEnumerator IEReturn(Vector3 originalPosition)
    {
        float elapsedTime = 0;
        float smoothTime = 0.1f;

        Vector3 start = bone.localPosition;

        while(elapsedTime < smoothTime)
        {
            bone.localPosition = Vector3.Lerp(start, originalPosition, elapsedTime / smoothTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        smoothCoroutine = null;
    }
}