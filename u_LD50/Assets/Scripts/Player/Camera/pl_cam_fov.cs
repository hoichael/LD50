using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_cam_fov : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    // caching for performance
    private int baseFov;

    void Start()
    {
        baseFov = pl_settings.Instance.FovBase;
        cam.fieldOfView = baseFov;
    }

    void Update()
    {
        cam.fieldOfView = baseFov + pl_state.Instance.currentFovOffsetSprint + pl_state.Instance.currentFovOffsetRecoil;
    }
}
