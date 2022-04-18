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

    protected override void OnEnable()
    {
        StartCoroutine(CheckPlayerDis());
     //   headTarget.SetParent(targetObj);
     //   chestTarget.SetParent(targetObj);

     //   headTarget.localPosition = chestTarget.localPosition = Vector3.zero;

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
        info.rb.velocity = Vector3.ClampMagnitude(info.rb.velocity, 3);
    }

    private void HandleRotation()
    {
        info.trans.LookAt(new Vector3(targetObj.position.x, 0, targetObj.position.z));
    }

    private IEnumerator CheckPlayerDis()
    {
        yield return new WaitForSeconds(checkInterval);

        if (Vector3.Distance(transform.position, targetObj.position) > engageDistance)
        {
            ChangeState("idle");
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
    }
}
