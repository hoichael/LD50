using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_cam_rot : MonoBehaviour
{
    [SerializeField]
    private Transform camHolder;

    [SerializeField]
    private pl_input input;

    [SerializeField]
    private Transform orientation;

//    [SerializeField]
//    private Transform toolsContainer;

    public float rotX, rotY;

    [SerializeField]
    private pl_box boxScript;

    private Vector3 initRot;

    [SerializeField]
    private Vector3 boxActiveRot;

    private Vector3 boxRotTarget;

    private bool transitionActive;
    private bool boxActive;

    [SerializeField]
    private Vector3 camHolderPosDefault;

    [SerializeField]
    private Vector3 camHolderPoxBox;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.localPosition = camHolderPosDefault;
    }

    public void InitBox()
    {
        initRot = transform.localRotation.eulerAngles;
        if (initRot.x > 90)
        {
            initRot.x = 0 - (360 - initRot.x);
        }

        boxRotTarget = boxActiveRot;
        boxRotTarget.y = transform.localRotation.eulerAngles.y;
 
        transitionActive = true;
    }

    public void InitCloseBox()
    {
        transitionActive = true;
        boxRotTarget = transform.localEulerAngles;
    }

    private void Update()
    {
        if(transitionActive)
        {
            HandleBoxTransition();
        }
        else
        {
            HandleInput();
            ApplyRotation();
        }
    }

    private void HandleBoxTransition()
    {
        transform.localRotation = Quaternion.Lerp(
            Quaternion.Euler(initRot),
            Quaternion.Euler(boxRotTarget),
            boxScript.transitionProgress);

        transform.localPosition = Vector3.Lerp(
            camHolderPosDefault,
            camHolderPoxBox,
            boxScript.transitionProgress);

        if(boxScript.transitionProgress == boxScript.currentTransitionTarget)
        {
            transitionActive = false;

            rotX = transform.localRotation.eulerAngles.x;
            if (rotX > 90)
            {
                rotX = 0 - (360 - rotX);
            }

            rotY = transform.localRotation.eulerAngles.y;

            if(boxScript.currentTransitionTarget == 1)
            {
                boxActive = true;
            }
            else
            {
                boxActive = false;
            }
        }
    }

    private void HandleInput()
    {
     //   if (boxActive) return;

        rotY += input.mouseX * pl_settings.Instance.mouseSens;
        rotX -= input.mouseY * pl_settings.Instance.mouseSens;

        if(boxActive)
        {
            rotX = Mathf.Clamp(rotX, 30f, 89.5f);
            rotY = Mathf.Clamp(rotY, boxRotTarget.y - 110f, boxRotTarget.y + 110f);
        }
        else
        {
            rotX = Mathf.Clamp(rotX, -89.5f, 89.5f);
        }
    }

    private void ApplyRotation()
    {
        // Rotate cam (mouse input based rotation + action based offset)
        camHolder.localRotation = Quaternion.Euler(rotX, rotY, pl_state.Instance.camTilt);

        // Apply action based offset to tool in hand
        //toolsContainer.localRotation = Quaternion.Euler(
        //    0,
        //    0,
        //    pl_state.Instance.camTilt * -3);

        // if box not active rotate orientation ref obj horizontally based on mouse input
        if (boxActive) return;
        orientation.transform.rotation = Quaternion.Euler(0, rotY, 0);
    }
}
