using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_goat_st_init : en_state_base
{
    protected override void OnEnable()
    {
        base.OnEnable();

        ChangeState("idle");
    }
}
