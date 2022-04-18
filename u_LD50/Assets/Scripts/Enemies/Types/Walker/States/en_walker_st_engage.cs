using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_walker_st_engage : en_state_base
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float checkInterval;

    [SerializeField]
    private Transform targetObj;

    [SerializeField]
    private float engageDistance;

    [SerializeField]
    private Transform headTarget, chestTarget;

    [SerializeField]
    private en_walker_info walkerInfo;

    [SerializeField]
    private en_walker_headbob bob;

    [SerializeField]
    private float attackDistance;

    protected override void OnEnable()
    {
        StartCoroutine(CheckPlayerDis());
        //   headTarget.SetParent(targetObj);
        //   chestTarget.SetParent(targetObj);

        //   headTarget.localPosition = chestTarget.localPosition = Vector3.zero;
        walkerInfo.currentlyWalking = true;
        bob.UpdateValues();

        targetObj = pl_state.Instance.GLOBAL_CAM_REF.transform;
    }

    private void Update()
    {

        HandleRotation();

        headTarget.position = chestTarget.position = targetObj.position;

     //   ChasePlayer();
    }

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        info.rb.AddForce((info.trans.forward * moveSpeed) * Time.deltaTime, ForceMode.Force);
        info.rb.velocity = Vector3.ClampMagnitude(info.rb.velocity, 4f);
    }

    private void HandleRotation()
    {
        info.trans.LookAt(new Vector3(pl_state.Instance.GLOBAL_CAM_REF.transform.position.x, info.trans.position.y, pl_state.Instance.GLOBAL_CAM_REF.transform.position.z));
    }

    private IEnumerator CheckPlayerDis()
    {
        yield return new WaitForSeconds(checkInterval);

        float dist = Vector3.Distance(transform.position, pl_state.Instance.GLOBAL_CAM_REF.transform.position);

        if (dist > engageDistance)
        {
            ChangeState("idle");
        }
        else if(dist < attackDistance)
        {
            ChangeState("attack");
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
        walkerInfo.currentlyWalking = false;
        bob.UpdateValues();
    }
}
