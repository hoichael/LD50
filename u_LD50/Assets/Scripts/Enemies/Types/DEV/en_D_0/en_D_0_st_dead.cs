using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_0_st_dead : en_state_base
{
    protected override void OnEnable()
    {
        base.OnEnable();
        info.trans.rotation = Quaternion.Euler(90, 0, 0);
    }
}
