using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_0_st_idle : en_state_base
{
    protected override void OnEnable()
    {
        base.OnEnable();
        print("en_D_0 enter idle");
    }

    private void Update()
    {
        info.trans.LookAt(new Vector3(pl_state.Instance.GLOBAL_PL_TRANS_REF.position.x, 0, pl_state.Instance.GLOBAL_PL_TRANS_REF.position.z));
    }
}
