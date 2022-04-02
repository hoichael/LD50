using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_cam_fov : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    void Start()
    {
        pl_state.Instance.camFov = pl_settings.Instance.FovBase;
    }

    void Update()
    {
        cam.fieldOfView = pl_state.Instance.camFov;
    }
}
