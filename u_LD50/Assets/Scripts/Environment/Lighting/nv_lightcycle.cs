using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_lightcycle : MonoBehaviour
{
    [SerializeField]
    private Transform sunAnchor;

    [SerializeField]
    private float rotationStep;

    private float currentRot;

    void Update()
    {
        currentRot += rotationStep;
        sunAnchor.localRotation = Quaternion.Euler(currentRot, 0, 0);
    }
}
