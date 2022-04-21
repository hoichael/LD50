using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_st_idle : en_state_base
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private en_spider_legs_ground legsGround;

    [SerializeField]
    private en_spider_legs_air legsAir;

    protected override void OnEnable()
    {
        base.OnEnable();

    //    legsAir.enabled = false;
    //    legsGround.enabled = true;

        StartCoroutine(HandleDuration());
    }

    private IEnumerator HandleDuration()
    {
        yield return new WaitForSeconds(duration);

        if(info.grounded)
        {
            ChangeState("move");
        }
        else
        {
            StartCoroutine(HandleDuration());
        }
    }
}
