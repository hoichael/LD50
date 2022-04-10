using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_spider_0_st_move : en_state_base
{
    [SerializeField]
    private float duration;

    private float targetRotY;

    private float currentRotY;

    private float initRotY;

    private float currentAnimProgress;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float turnSpeed;

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(HandleDuration());
        InitRotation();
    }

    private void Update()
    {
        HandleRotation();
    }

    private void FixedUpdate()
    {
        info.rb.AddForce(info.trans.forward * moveSpeed, ForceMode.Acceleration);
    }

    private void InitRotation()
    {
        targetRotY = Random.Range(30f, 120f);
        currentAnimProgress = 0;
        initRotY = info.trans.localEulerAngles.y;
    }

    private void HandleRotation()
    {
        if (currentAnimProgress == 1) return;

        currentAnimProgress = Mathf.MoveTowards(currentAnimProgress, 1, turnSpeed * Time.deltaTime);

        currentRotY = Mathf.Lerp(
            initRotY,
            targetRotY,
            currentAnimProgress
            );

        info.trans.localRotation = Quaternion.Euler(new Vector3(
            transform.localEulerAngles.x,
            currentRotY,
            transform.localEulerAngles.z
            ));
    }

    private IEnumerator HandleDuration()
    {
        yield return new WaitForSeconds(duration);

        ChangeState("idle");
    }
}
