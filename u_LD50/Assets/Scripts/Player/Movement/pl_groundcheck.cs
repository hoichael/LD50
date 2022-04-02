using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_groundcheck : MonoBehaviour
{
    [SerializeField]
    private Transform groundCheckTrans;

    private int groundLayerMask;

    private void Start()
    {
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        pl_state.Instance.grounded = GetGroundedBool();
    }

    private bool GetGroundedBool()
    {
        return Physics.CheckSphere(
            groundCheckTrans.position,
            pl_settings.Instance.groundCheckRadius,
            groundLayerMask);
    }
}
