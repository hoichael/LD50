using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_move : MonoBehaviour
{
    [SerializeField]
    private pl_input input;

    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private Rigidbody rb;

    private Vector3 currentDir;

    private float currentMult;

    private bool currentlySprinting;

    //caching for performance
    private float sprintMult, hungerMult;

    private void Start()
    {
        rb.freezeRotation = true;
        sprintMult = pl_settings.Instance.moveSprintMult;
        hungerMult = pl_settings.Instance.hugerMultSprint;
    }

    private void Update()
    {
        GetMoveDirection();
        CalcCamTilt();
        CalcCamFOV();
    }

    private void FixedUpdate()
    {
        SetStateRelatedValues();
        ApplyMovement();
    }

    private void SetStateRelatedValues()
    {
        // this is ugly (even for jam code =D)
        currentlySprinting = false;

        if(pl_state.Instance.grounded)
        {
            rb.drag = pl_settings.Instance.dragGround;

            if(input.sprintKey && input.moveY > 0)
            {
                currentMult = sprintMult;
                pl_state.Instance.currentHungerMult = hungerMult;
                currentlySprinting = true;
            }
            else
            {
                currentMult = 1;
                pl_state.Instance.currentHungerMult = 1;
            }
        }
        else
        {
            rb.drag = pl_settings.Instance.dragAir;
            currentMult = pl_settings.Instance.moveAirMult;
        }
    }

    private void GetMoveDirection()
    {
        currentDir = orientation.forward * input.moveY + orientation.right * input.moveX;
        currentDir = currentDir.normalized;
    }

    private void ApplyMovement()
    {
        rb.AddForce(currentDir * pl_settings.Instance.moveSpeed * currentMult, ForceMode.Acceleration);
    }

    private void CalcCamTilt()
    {
        pl_state.Instance.camTilt = Mathf.Lerp(
            pl_state.Instance.camTilt, 
            input.moveX * pl_settings.Instance.camTiltMoveAmount,
            pl_settings.Instance.camTiltMoveFactor * Time.deltaTime);
    }

    private void CalcCamFOV()
    {
        float valToLerpTo = currentlySprinting ? pl_settings.Instance.FovSprintAmount : 0;

        pl_state.Instance.currentFovOffsetSprint = Mathf.Lerp(
            pl_state.Instance.currentFovOffsetSprint,
            valToLerpTo,
            pl_settings.Instance.FovSprintFactor * Time.deltaTime);
    }
}
