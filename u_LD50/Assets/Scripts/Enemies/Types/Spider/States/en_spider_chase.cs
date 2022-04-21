using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_chase : en_state_base
{
    [SerializeField]
    private float turnSpeed;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpTriggerDist;

    private Transform targetTrans;
    private Vector3 dirToTarget;
    private Quaternion targetRot;

    private void Start()
    {
        targetTrans = pl_state.Instance.GLOBAL_CAM_REF.transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        info.anim.SetBool("walk", true);
    }

    private void Update()
    {
        HandleRotation();
        CheckPlayerDist();

        info.rb.velocity = new Vector3(info.trans.forward.x * moveSpeed, info.rb.velocity.y, info.trans.forward.z * moveSpeed);
    }

    private void HandleRotation()
    {
        dirToTarget = (
            new Vector3(targetTrans.position.x, info.trans.position.y, targetTrans.position.z)
            - info.trans.position).normalized;

        targetRot = Quaternion.LookRotation(dirToTarget);
        info.trans.rotation = Quaternion.Slerp(info.trans.rotation, targetRot, turnSpeed * Time.deltaTime);
    }

    private void CheckPlayerDist()
    {
        if (Vector3.Distance(info.trans.position, targetTrans.position) < jumpTriggerDist)
        {
            ChangeState("jump");
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        info.anim.SetBool("walk", false);
    }
}
