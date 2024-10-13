using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBobbing : MonoBehaviour
{
    private const float LOW_INTERVAL = (Mathf.PI * 3) / 2;

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
        float stepTime = 0;
        float speed;
        Vector3 originalPosition = bone.localPosition;

        while (PlayerIsMoving)
        {
            if (PlayerInputHandler.SprintInput)
                speed = bobbingSpeed * 1.5f;
            else
                speed = bobbingSpeed;

            float stepInterval = LOW_INTERVAL / speed;

            if (stepTime > stepInterval)
            {
                stepTime -= stepInterval;
                AudioManager.PlaySound(AudioManager.StepSound, 0.85f, 1f, 0.15f);
            }

            Vector3 position = bone.localPosition;
            float offset = (0.01f * bobbingHeight) * Mathf.Sin(speed * 1.25f * t);
            position.y += offset;

            bone.localPosition = position;

            t += Time.deltaTime;
            stepTime += Time.deltaTime;

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