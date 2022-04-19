using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_swiper_init : en_state_base
{
//    [SerializeField]
//    private Transform lookTargetTrans;

    protected override void OnEnable()
    {
        base.OnEnable();

     //   lookTargetTrans.SetParent(pl_state.Instance.GLOBAL_CAM_REF.transform);
     //   lookTargetTrans.localPosition = Vector3.zero;

        ChangeState("chase");
    }
}
