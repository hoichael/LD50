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

    private void Awake()
    {
        rb.freezeRotation = true;
    }

    private void Update()
    {
        GetMoveDirection();
        CalcCamTilt();
    }

    private void FixedUpdate()
    {
        SetStateRelatedValues();
        ApplyMovement();
    }

    private void SetStateRelatedValues()
    {
        if(pl_state.Instance.grounded)
        {
            rb.drag = pl_settings.Instance.dragGround;
            currentMult = 1;
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
}
