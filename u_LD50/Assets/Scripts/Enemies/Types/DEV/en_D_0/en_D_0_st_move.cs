using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_0_st_move : en_state_base
{

    [SerializeField]
    private float moveSpeed;

    void Update()
    {
        info.rb.AddForce(info.trans.forward * (moveSpeed * Time.deltaTime), ForceMode.Force);

        if(Vector3.Distance(info.trans.position, pl_state.Instance.GLOBAL_PL_TRANS_REF.position) < 2f)
        {
            ChangeState("attack");
        }
    }
}
