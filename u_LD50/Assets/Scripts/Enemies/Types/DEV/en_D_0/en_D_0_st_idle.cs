using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_0_st_idle : en_state_base
{
    [SerializeField]
    private float attackTriggerRange;

    [SerializeField]
    private float moveRangeCheck;

    [SerializeField]
    private float rangeCheckInterval;

    protected override void OnEnable()
    {
        base.OnEnable();
        print("en_D_0 enter " + stateID);
        StartCoroutine(PlayerRangeCheck());
    }

    private IEnumerator PlayerRangeCheck()
    {
        yield return new WaitForSeconds(rangeCheckInterval);

        float distToPlayer = Vector3.Distance(pl_state.Instance.GLOBAL_PL_TRANS_REF.position, info.trans.position);

        if (distToPlayer < attackTriggerRange)
        {
            ChangeState("attack");
        }
        else if(distToPlayer < moveRangeCheck)
        {
            ChangeState("move");
        }
        else
        {
            StartCoroutine(PlayerRangeCheck());
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }
}
