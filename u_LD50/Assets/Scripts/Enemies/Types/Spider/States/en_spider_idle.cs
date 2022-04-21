using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_idle : en_state_base
{
    [SerializeField]
    private float checkInterval;

    [SerializeField]
    private float triggerDistEngage;

    private Transform targetObj;

    private void Start()
    {
        targetObj = pl_state.Instance.GLOBAL_PL_TRANS_REF;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(CheckPlayerDist());
        info.anim.SetBool("idle", true);
    }

    private IEnumerator CheckPlayerDist()
    {
        yield return new WaitForSeconds(checkInterval);

        float dist = Vector3.Distance(info.trans.position, targetObj.position);

        if (dist < triggerDistEngage)
        {
            ChangeState("chase");
        }
        else
        {
            StartCoroutine(CheckPlayerDist());
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        info.anim.SetBool("idle", false);
    }
}
