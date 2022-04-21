using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_st_jump : en_state_base
{
    [SerializeField]
    private en_spider_legs_ground legsGround;

    [SerializeField]
    private en_spider_legs_air legsAir;

    [SerializeField]
    private en_spider_groundcheck groundcheck;

    [SerializeField]
    private float jumpForceX, jumpForceY;

    protected override void OnEnable()
    {
        base.OnEnable();

        info.rb.useGravity = true;
        info.grounded = false;

        legsAir.enabled = true;
        legsGround.enabled = false;



        info.rb.AddForce(Vector3.up * jumpForceY, ForceMode.Impulse);
        //   info.rb.AddForce(info.trans.forward * 2f, ForceMode.Impulse);

        //   groundcheck.enabled = true;

        StartCoroutine(GroundCheckDelay());
    }

    private IEnumerator GroundCheckDelay()
    {
        yield return new WaitForSeconds(0.1f);
        groundcheck.enabled = true;
        ChangeState("idle");
    }
}
