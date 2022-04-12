using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_cam_bob : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rbPlayer;

    [SerializeField, Range(0, 0.1f)]
    private float amplitude;

    [SerializeField, Range(0, 40)]
    private float frequency;

    [SerializeField]
    private float resetLerpFactor;

    [SerializeField]
    private bool stabilize;

    [SerializeField]
    private float stabilizeTargetDist;

    [SerializeField]
    private Transform camTrans;

    [SerializeField]
    private Transform camHolderTrans;

    private float triggerSpeed = 3.4f;
    private Vector3 initPos;

    private pl_input input;

    [SerializeField]
    private pl_item_sway itemSway;

    private void Start()
    {
        initPos = camTrans.localPosition;
    }

    private void Update()
    {
        if(TimeToBob())
        {
            ApplyMotion(CalcMotion());
        }

        ResetBob();

        if (stabilize) StabilizeView();
    }

    private bool TimeToBob()
    {
        return (pl_state.Instance.grounded && rbPlayer.velocity.magnitude > triggerSpeed);
    }

    private Vector3 CalcMotion()
    {
        Vector3 newPos = Vector3.zero;

        newPos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude;
        newPos.y += Mathf.Sin(Time.time * frequency) * amplitude * 1.4f;

        return newPos;
    }

    private void ApplyMotion(Vector3 newPos)
    {
        camTrans.localPosition += newPos;

        itemSway.currentOffsetWalkPos = new Vector3(newPos.x * -0.44f, newPos.y * -0.58f, 0);
    }

    private void ResetBob()
    {
        if (camTrans.localPosition == initPos) return;
        camTrans.localPosition = Vector3.Lerp(camTrans.localPosition, initPos, resetLerpFactor * Time.deltaTime);
    }

    private void StabilizeView()
    {
        // ugly but whatever
        if (pl_state.Instance.camTiltDmg != 0) return;

        // calc stabilization
        Vector3 stabilizedView = new Vector3(
            rbPlayer.transform.position.x,
            rbPlayer.transform.position.y + camHolderTrans.localPosition.y,
            rbPlayer.transform.position.z) 
            + camHolderTrans.forward * stabilizeTargetDist;

        //apply stabilization
        camTrans.LookAt(stabilizedView);
    }
}
