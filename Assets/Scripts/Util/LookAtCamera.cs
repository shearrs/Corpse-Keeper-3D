using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;
    private Transform target;

    private void Awake()
    {
        target = Camera.main.transform;
    }

    private void Update()
    {
        if (invert)
        {
            Vector3 direction = -(target.position - transform.position);
            transform.forward = direction;
        }
        else
            transform.LookAt(target);
    }
}
