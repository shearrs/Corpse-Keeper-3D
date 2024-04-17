using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class OscillateScale : MonoBehaviour
{
    [SerializeField] private float lowRange;
    [SerializeField] private float highRange;
    [SerializeField] private float rate;

    private void Start()
    {
        StartCoroutine(IEOscillate());
    }

    private IEnumerator IEOscillate()
    {
        while(true)
        {
            float elapsedTime = 0;
            float range = Random.Range(lowRange, highRange);

            Vector3 end = new(range, range, range);
            Vector3 start = transform.localScale;

            while (elapsedTime < rate)
            {
                while (rate == 0)
                    yield return null;

                float percent = elapsedTime / rate;

                transform.localScale = Vector3.Lerp(start, end, percent);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
        }
    }
}
