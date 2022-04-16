using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_st_init : en_state_base
{
    [SerializeField]
    private en_spider_legs_ground legsGround;

    [SerializeField]
    private en_spider_legs_air legsAir;

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(HandleDuration());
    }

    private IEnumerator HandleDuration()
    {
        yield return new WaitForSeconds(0.01f);

        if(info.grounded)
        {
            legsGround.enabled = true;
        }
        else
        {
            legsAir.enabled = true;
        }

        ChangeState("idle");
    }
}
