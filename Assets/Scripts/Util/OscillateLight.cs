using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OscillateLight : MonoBehaviour
{
    [SerializeField] private Light target;
    [SerializeField] private float lowRange;
    [SerializeField] private float highRange;
    [SerializeField] private float rate;

    private void Start()
    {
        StartCoroutine(IEOscillate());
    }

    private IEnumerator IEOscillate()
    {
        while (true)
        {
            float elapsedTime = 0;

            float end = Random.Range(lowRange, highRange);
            float start = target.range;

            while (elapsedTime < rate)
            {
                while (rate == 0)
                    yield return null;

                float percent = elapsedTime / rate;

                target.range = Mathf.Lerp(start, end, percent);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
        }
    }
}
