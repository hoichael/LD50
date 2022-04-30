using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_knock : en_state_base
{
    private bool checkForGround;

    [SerializeField]
    private Transform groundCheckTrans;

    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private float groundCheckRadius;

    protected override void OnEnable()
    {
        base.OnEnable();
        info.anim.SetBool("jump", true);
        StartCoroutine(GroundCheckTimer());
    }

    private void Update()
    {
        if (checkForGround) GroundCheck();
    }

    private void GroundCheck()
    {
        /*
        if (Physics.CheckSphere(
            groundCheckTrans.position,
            groundCheckRadius,
            groundLayerMask))
        {
            checkForGround = false;
            ChangeState("chase");
        }
        */
        if (Physics.CheckSphere(
            info.trans.position - Vector3.down,
            groundCheckRadius,
            groundLayerMask))
        {
            checkForGround = false;
            ChangeState("chase");
        }
    }

    private IEnumerator GroundCheckTimer()
    {
        yield return new WaitForSeconds(0.2f);
        checkForGround = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        info.anim.SetBool("jump", false);
    }
}
