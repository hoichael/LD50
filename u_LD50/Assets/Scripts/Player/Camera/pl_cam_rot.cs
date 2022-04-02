using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_cam_rot : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private pl_input input;

    [SerializeField]
    private Transform orientation;

    private float rotX, rotY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleInput();
        ApplyRotation();
    }

    private void HandleInput()
    {
        // db -----------------
        print("ROT X" + rotX);
        print("ROT Y: " + rotY);

        rotY += input.mouseX * pl_settings.Instance.mouseSens;
        rotX -= input.mouseY * pl_settings.Instance.mouseSens;

        rotX = Mathf.Clamp(rotX, -90f, 90f);
    }

    private void ApplyRotation()
    {
        cam.transform.localRotation = Quaternion.Euler(rotX, rotY, 0);
        orientation.transform.rotation = Quaternion.Euler(0, rotY, 0);
    }
}
