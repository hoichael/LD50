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

    [SerializeField]
    private en_walker_info walkerInfo;

    protected override void OnEnable()
    {
        targetObj = pl_state.Instance.GLOBAL_CAM_REF.transform;
        StartCoroutine(CheckPlayerDis());
        legs.enabled = false;
    }

    private IEnumerator CheckPlayerDis()
    {
     //   print(targetObj.position);
        yield return new WaitForSeconds(checkInterval);

        if(Vector3.Distance(transform.position, pl_state.Instance.GLOBAL_CAM_REF.transform.position) < engageDistance)
        {
            losRayOrigin.LookAt(pl_state.Instance.GLOBAL_CAM_REF.transform);

            RaycastHit hit;
            if (Physics.Raycast(losRayOrigin.position, losRayOrigin.forward, out hit, engageDistance + 2, ~enemiesLayerMask))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    //   ChangeState("engage");
                    ChangeState("turn");
                }
                else
                {
                    StartCoroutine(CheckPlayerDis());
                }
            }
            else
            {
                StartCoroutine(CheckPlayerDis());
            }
        }
        else
        {
            StartCoroutine(CheckPlayerDis()); // for some reason StopAllCoroutines isnt working
        }
        //         StartCoroutine(CheckPlayerDis());
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        legs.enabled = true;
    }

}
