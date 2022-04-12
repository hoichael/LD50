using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_item_sway : MonoBehaviour
{
    [Header("Refs")]

    [SerializeField]
    private Transform itemContainer;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private pl_input input;

    [Header("Look Sway")]

    [SerializeField]
    private float swayStep;

    [SerializeField]
    private float swayMult;

    private Quaternion currentSwayLook;

    [Header("Walk Bob (Set in pl_cam_bob)")]

    public Vector3 currentOffsetWalkPos;



    [Header("Impact Bob")]

    [SerializeField]
    private float impactOffsetPosY;

    [SerializeField]
    private float impactLerpSpeed;

    private float impactCurrentProgress;

    private Vector3 currentOffsetImpactPos;

    private bool lastFrameGrounded;

    [Header("Impact Sway")]

    [SerializeField]
    private float impactOffsetRotX;

    private float currentOffsetImpactRot;


    private void Update()
    {

        if(lastFrameGrounded != pl_state.Instance.grounded)
        {
            InitImpactBob();
        }
        lastFrameGrounded = pl_state.Instance.grounded;


        CalcLookSway();

        ApplyOffsets();
    }

    private void InitImpactBob()
    {
        if (lastFrameGrounded == pl_state.Instance.grounded) return;

        lastFrameGrounded = pl_state.Instance.grounded;
    }

    private void HandleImpactBob()
    {

    }

    private void CalcLookSway()
    {
        float rotX = input.mouseX * swayMult;
        float rotY = input.mouseY * swayMult;

        Quaternion targetRotX = Quaternion.AngleAxis(-rotY, Vector3.right);
        Quaternion targetRotY = Quaternion.AngleAxis(rotX, Vector3.up);
        Quaternion targetRotZ = Quaternion.AngleAxis(pl_state.Instance.camTiltMove * -2.65f, Vector3.forward);

        currentSwayLook = targetRotX * targetRotY *  targetRotZ;
    }

    private void ApplyOffsets()
    {
        /*
        itemContainer.localRotation = Quaternion.Euler(
            0,
            0,
            pl_state.Instance.camTilt * -2.4f);
        */

        itemContainer.localRotation = Quaternion.Slerp(
            itemContainer.localRotation,
            currentSwayLook,
            swayStep * Time.deltaTime
            );

        itemContainer.localPosition = Vector3.zero + currentOffsetImpactPos + currentOffsetWalkPos; // + other offsets
    }
}
