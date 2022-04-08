using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_box : MonoBehaviour
{
    [SerializeField]
    private pl_move move;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Transform anchor;

    [SerializeField]
    private GameObject container;

    [SerializeField]
    private pl_cam_rot camRot;

    private List<GameObject> currentObjects;

    private bool currentlyActive;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && pl_state.Instance.grounded)
        {
            if(currentlyActive)
            {
                CloseBox();
            }
            else
            {
                OpenBox();
            }
        }
    }

    private void OpenBox()
    {
        move.currentMult = 0;
        camRot.lookAtBox = true;
        rb.velocity = Vector3.zero;
        currentlyActive = true;
    //    rb.isKinematic = true;
    }

    private void CloseBox()
    {
        move.currentMult = 1;
        camRot.lookAtBox = false;
        camRot.resetBoxLook = true;
        currentlyActive = false;
        //   rb.isKinematic = false;
    }

}
