using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_groundcheck : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private dmg_base dmgInfo;

    [SerializeField]
    private pl_health_damage healthDamage;

    [SerializeField]
    private float fallDmgMultLow, fallDmgAmountMid, fallDmgMultHigh;

    [SerializeField]
    private Transform groundCheckTrans;

    [SerializeField]
    private LayerMask groundLayerMask;

    private bool groundedLastFrame;

    private void Update()
    {
        pl_state.Instance.grounded = GetGroundedBool();

        if(pl_state.Instance.grounded != groundedLastFrame)
        {
            CheckForFallDamage();
        }

        groundedLastFrame = pl_state.Instance.grounded;
    }

    private void CheckForFallDamage()
    {
    //    print(rb.velocity.y);
        if(rb.velocity.y < -14.4f)
            {
                if(rb.velocity.y < -17f)
                {
                    dmgInfo.dmgAmount =  Mathf.RoundToInt(rb.velocity.y * -fallDmgMultHigh);
                }
                else if(rb.velocity.y < -15.4f)
                {
                    dmgInfo.dmgAmount = Mathf.RoundToInt(rb.velocity.y * -fallDmgAmountMid);
                }
                else
                {
                    dmgInfo.dmgAmount = Mathf.RoundToInt(rb.velocity.y * -fallDmgMultLow);
                }

                healthDamage.HandleDamage(dmgInfo);
            }
    }
    private bool GetGroundedBool()
    {
        return Physics.CheckSphere(
            groundCheckTrans.position,
            pl_settings.Instance.groundCheckRadius,
            groundLayerMask);
    }
}
