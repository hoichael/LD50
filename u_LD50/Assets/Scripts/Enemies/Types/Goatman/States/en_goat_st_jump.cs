using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_goat_st_jump : en_state_base
{
    [SerializeField]
    private float jumpForceX, jumpForceY;

    [SerializeField]
    private float turnSpeed;
    private bool turnToPlayer;
    private Transform targetTrans;
    private Vector3 dirToTarget;
    private Quaternion targetRot;

    private bool checkForGround;

    private void Start()
    {
        targetTrans = pl_state.Instance.GLOBAL_PL_TRANS_REF;    
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        turnToPlayer = true;
        info.anim.SetBool("jump_charge", true);
        StartCoroutine(JumpUpTimer());
    }

    private void Update()
    {

        if (!turnToPlayer)
        {
            TurnToPlayer();
        }
        else if(checkForGround)
        {
            GroundCheck();
        }

    }

    private void TurnToPlayer()
    {
        dirToTarget = (
            new Vector3(targetTrans.position.x, info.trans.position.y, targetTrans.position.z)
            - info.trans.position).normalized;

        targetRot = Quaternion.LookRotation(dirToTarget);
        info.trans.rotation = Quaternion.Slerp(info.trans.rotation, targetRot, turnSpeed * Time.deltaTime);
    }

    private void InitJump()
    {
        info.rb.AddForce(info.trans.forward * jumpForceX, ForceMode.Impulse);
        info.rb.AddForce(info.trans.up * jumpForceY, ForceMode.Impulse);
    }
    private void InitLand()
    {

    }

    private void GroundCheck()
    {

    }


    private IEnumerator JumpUpTimer()
    {
        yield return new WaitForSeconds(0.2f);

        info.anim.SetBool("jump_charge", false);
        info.anim.SetBool("jump_up", true);
        turnToPlayer = false;

        InitJump();
    }



    protected override void OnDisable()
    {
        base.OnDisable();
        info.anim.SetBool("jump_land", false);
        StopAllCoroutines();
    }

}
