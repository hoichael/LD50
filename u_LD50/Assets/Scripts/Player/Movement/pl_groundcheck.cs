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

    [SerializeField]
    private FMODUnity.EventReference sfxFootstep;

    [SerializeField]
    private FMODUnity.EventReference sfxFallDamage;

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
        float vel = rb.velocity.y;

        if(vel < -2.3f)
        {
            FMODUnity.RuntimeManager.PlayOneShot(sfxFootstep);

            if (vel < -14.4f)
            {
                if (vel < -17f)
                {
                    dmgInfo.dmgAmount = Mathf.RoundToInt(vel * -fallDmgMultHigh);
                }
                else if (vel < -15.4f)
                {
                    dmgInfo.dmgAmount = Mathf.RoundToInt(vel * -fallDmgAmountMid);
                }
                else
                {
                    dmgInfo.dmgAmount = Mathf.RoundToInt(vel * -fallDmgMultLow);
                }

                FMODUnity.RuntimeManager.PlayOneShot(sfxFallDamage);
                healthDamage.HandleDamage(dmgInfo);
            }
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
