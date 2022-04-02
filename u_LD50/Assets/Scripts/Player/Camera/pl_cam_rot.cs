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

    [SerializeField]
    private Transform toolsContainer;

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
        // Rotate cam (mouse input based rotation + action based offset)
        cam.transform.localRotation = Quaternion.Euler(rotX, rotY, pl_state.Instance.camTilt);

        // Apply action based offset to tool in hand
        toolsContainer.localRotation = Quaternion.Euler(
            0,
            0,
            pl_state.Instance.camTilt * -1);

        // Rotate orientation ref obj horizontally based on mouse input
        orientation.transform.rotation = Quaternion.Euler(0, rotY, 0);
    }
}
