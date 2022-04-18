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

    [SerializeField]
    private Transform losRayOrigin;

    [SerializeField]
    private LayerMask enemiesLayerMask;

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
            losRayOrigin.LookAt(targetObj);

            RaycastHit hit;
            if (Physics.Raycast(losRayOrigin.position, losRayOrigin.forward, out hit, engageDistance + 2, ~enemiesLayerMask))
            {
                print(hit.transform.tag);
                if (hit.transform.CompareTag("Player"))
                {
                    ChangeState("engage");
                }
            }
        }
        
        StartCoroutine(CheckPlayerDis());
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        legs.enabled = true;
    }

}
