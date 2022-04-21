using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_jump : en_state_base
{
    [SerializeField]
    private float jumpForceX, jumpForceY;

    [SerializeField]
    private float turnSpeed;

    private Transform targetTrans;

    private bool checkForGround;

    [SerializeField]
    private Transform groundCheckTrans;

    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private float groundCheckRadius;

    [SerializeField]
    private float attackTriggerDist;

    private bool checkForAttack;

    [SerializeField]
    private float maxAttackAngle;

    private void Start()
    {
        targetTrans = pl_state.Instance.GLOBAL_CAM_REF.transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        checkForAttack = true;
        info.anim.SetBool("jump", true);
        InitJump();
     //   StartCoroutine(JumpUpTimer());
    }

    private void Update()
    {
        if (checkForGround)
        {
            GroundCheck();

            if(checkForAttack) CheckForPlayer();
        }
    }

    private void CheckForPlayer()
    {
        if (Vector3.Distance(info.trans.position, targetTrans.position) < attackTriggerDist)
        {
            checkForAttack = false;

            if (Vector3.Angle(targetTrans.forward, info.trans.position - targetTrans.position) < maxAttackAngle)
            {
                  ChangeState("attack");
            }

        }
    }

    private void InitJump()
    {
        info.rb.AddForce(info.trans.forward * jumpForceX, ForceMode.Impulse);
        info.rb.AddForce(info.trans.up * jumpForceY, ForceMode.Impulse);

        StartCoroutine(GroundCheckTimer());
    }

    private void GroundCheck()
    {
        if (Physics.CheckSphere(
            groundCheckTrans.position,
            groundCheckRadius,
            groundLayerMask))
        {
            checkForGround = false;
            print("SPIDER GROUND");
            ChangeState("chase");
         //   InitLand();
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
        StopAllCoroutines();
    }
}
