using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_goat_st_attack : en_state_base
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private float turnSpeed;
    private Transform targetTrans;
    private Vector3 dirToTarget;
    private Quaternion targetRot;

    private void Start()
    {
        targetTrans = pl_state.Instance.GLOBAL_PL_TRANS_REF;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        info.anim.SetBool("attack", true);
        StartCoroutine(HandleDuration());
    }

    private void Update()
    {
        dirToTarget = (
            new Vector3(targetTrans.position.x, info.trans.position.y, targetTrans.position.z)
            - info.trans.position).normalized;

        targetRot = Quaternion.LookRotation(dirToTarget);
        info.trans.rotation = Quaternion.Slerp(info.trans.rotation, targetRot, turnSpeed * Time.deltaTime);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        info.anim.SetBool("attack", false);
    }

    private IEnumerator HandleDuration()
    {
        yield return new WaitForSeconds(duration);

        ChangeState("idle");
    }
}
