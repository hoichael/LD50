using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_spider_0_st_idle : en_state_base
{
    [SerializeField]
    private float duration;

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(HandleDuration());
    }

    private IEnumerator HandleDuration()
    {
        yield return new WaitForSeconds(duration);

        ChangeState("move");
    }
}
