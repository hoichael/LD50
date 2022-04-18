using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_walker_st_idle : en_state_base
{
    [SerializeField]
    private float checkInterval;

    [SerializeField]
    private Transform targetObj;

    [SerializeField]
    private float engageDistance;

    [SerializeField]
    private en_walker_legs legs; // toggle bc performance

    protected override void OnEnable()
    {
        StartCoroutine(CheckPlayerDis());
        legs.enabled = false;
    }

    private IEnumerator CheckPlayerDis()
    {
        yield return new WaitForSeconds(checkInterval);

        if(Vector3.Distance(transform.position, targetObj.position) < engageDistance)
        {
            ChangeState("engage");
        }
        else
        {
            StartCoroutine(CheckPlayerDis());
        }
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        legs.enabled = false;
    }

}
