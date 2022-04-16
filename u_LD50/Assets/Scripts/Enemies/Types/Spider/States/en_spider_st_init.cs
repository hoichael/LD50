using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_st_init : en_state_base
{
    [SerializeField]
    private en_spider_groundcheck groundcheck;

    [SerializeField]
    private en_spider_legs_ground legsGround;

    [SerializeField]
    private en_spider_legs_air legsAir;

    protected override void OnEnable()
    {
        base.OnEnable();

        groundcheck.enabled = true;
        ChangeState("idle");
    //    StartCoroutine(HandleDuration());
    }

    /*
    private IEnumerator HandleDuration()
    {
        yield return new WaitForSeconds(0.03f);

        if(info.grounded)
        {
            //    legsGround.enabled = true;
            groundcheck.enabled = false;
        }
        else
        {
        //    legsAir.enabled = true;
        }

        ChangeState("idle");
    }
    */
}
