using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_lightcycle : MonoBehaviour
{
    [SerializeField]
    private Transform sunAnchor;

    private float anchorRotX, anchorRotY;

    void Update()
    {
        anchorRotX += 0.04f;
        sunAnchor.localRotation = Quaternion.Euler(anchorRotX, 0, 0);
    }
}
