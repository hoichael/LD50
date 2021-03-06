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

    public float currentMult;

    private bool currentlySprinting;

    private RaycastHit hit;

    //caching for performance
    private float sprintMult; 
    private int hungerMult;

    [SerializeField]
    private float maxSlopeNormal;

    /*
    [SerializeField]
    private float maxGroundSpeedWalk;

    [SerializeField]
    private float maxGroundSpeedSprint;
    */

    private void Start()
    {
        rb.freezeRotation = true;
        sprintMult = pl_settings.Instance.moveSprintMult;
        hungerMult = pl_settings.Instance.hungerMultSprint;
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
         //   rb.drag = pl_settings.Instance.dragGround;

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
         //   rb.drag = pl_settings.Instance.dragAir;
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
        if (pl_state.Instance.grounded && Physics.Raycast(transform.position, Vector3.down, out hit, 3))
        {
            /*
            if(hit.normal != Vector3.up)
            {
                currentDir = Vector3.ProjectOnPlane(currentDir, hit.normal).normalized;
            }
            */

            if (Mathf.Abs(hit.normal.x) < maxSlopeNormal && Mathf.Abs(hit.normal.z) < maxSlopeNormal)
            {
                currentDir = Vector3.ProjectOnPlane(currentDir, hit.normal).normalized;
            }
        }

        rb.AddForce(currentDir * pl_settings.Instance.moveSpeed * currentMult, ForceMode.Acceleration);

        if (pl_state.Instance.grounded)
        {
            if (currentlySprinting)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, pl_settings.Instance.maxGroundSpeedSprint);
            }
            else
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, pl_settings.Instance.maxGroundSpeedWalk);
            }
        }
    }

    private void CalcCamTilt()
    {
        pl_state.Instance.camTiltMove = Mathf.Lerp(
            pl_state.Instance.camTiltMove, 
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
