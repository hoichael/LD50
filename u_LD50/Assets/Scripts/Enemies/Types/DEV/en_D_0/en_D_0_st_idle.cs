using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_0_st_idle : en_state_base
{
    [SerializeField]
    private float attackTriggerRange;

    [SerializeField]
    private float rangeCheckInterval;

    protected override void OnEnable()
    {
        base.OnEnable();
        print("en_D_0 enter " + stateID);
        StartCoroutine(PlayerRangeCheck());
    }

    private void Update()
    {
        info.trans.LookAt(new Vector3(pl_state.Instance.GLOBAL_PL_TRANS_REF.position.x, 0, pl_state.Instance.GLOBAL_PL_TRANS_REF.position.z));
    }

    private IEnumerator PlayerRangeCheck()
    {
        yield return new WaitForSeconds(rangeCheckInterval);

        if(Vector3.Distance(pl_state.Instance.GLOBAL_PL_TRANS_REF.position, info.trans.position) < attackTriggerRange)
        {
            ChangeState("attack");
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
