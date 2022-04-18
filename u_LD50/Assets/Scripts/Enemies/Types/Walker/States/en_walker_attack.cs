using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_walker_attack : en_state_base
{
    [SerializeField]
    private dmg_base dmgInfo;

    [SerializeField]
    private float animSpeed;

    [SerializeField]
    private AnimationCurve animCurve;

    private float currentAnimProgress;

    private int currentLerpTarget;

    [SerializeField]
    private Transform attackPosTrans;

    [SerializeField]
    private Vector3 attackColHalfExtents;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private en_walker_headbob bob;

    [SerializeField]
    private Transform handLTarget, handRTarget, headPosTarget;

    private Vector3 handLDefault, handLGoal, handRDefault, handRGoal, headPosDefault, headPosGoal;

    protected override void OnEnable()
    {
        base.OnEnable();

        currentAnimProgress = 0;
        currentLerpTarget = 1;
        info.rb.useGravity = false;
        info.rb.velocity = Vector3.zero;

        bob.enabled = false;

        handLDefault = handLTarget.position;
        handLGoal = pl_state.Instance.GLOBAL_CAM_REF.transform.position + new Vector3(-0.2f, 0.25f, 0.2f);
        handRGoal = pl_state.Instance.GLOBAL_CAM_REF.transform.position + new Vector3(0.2f, 0.1f, -0.2f);

        handRDefault = handRTarget.position;

        headPosDefault = headPosTarget.position;
        headPosGoal = headPosDefault + headPosTarget.forward * 1.5f;
    }

    private void Update()
    {
        currentAnimProgress = Mathf.MoveTowards(currentAnimProgress, currentLerpTarget, animSpeed * Time.deltaTime);

        headPosTarget.position = Vector3.Lerp(
            headPosDefault,
            headPosGoal,
            animCurve.Evaluate(currentAnimProgress)
            );

        handLTarget.position = Vector3.Lerp(
            handLDefault,
            handLGoal,
            animCurve.Evaluate(currentAnimProgress)
            );

        handRTarget.position = Vector3.Lerp(
            handRDefault,
            handRGoal,
            animCurve.Evaluate(currentAnimProgress)
            );

        if (currentAnimProgress == currentLerpTarget)
        {
            if(currentLerpTarget == 1)
            {
                StartCoroutine(AttackDelay());
            }
            else
            {
                ChangeState("turn");
            }
        }
    }

    private IEnumerator AttackDelay()
    {
        HandleHitbox();

        yield return new WaitForSeconds(0.3f);

        currentLerpTarget = 0;
    }

    private void HandleHitbox()
    {
        Collider[] hitColliders = Physics.OverlapBox(attackPosTrans.position, attackColHalfExtents, Quaternion.identity, playerLayerMask);
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].isTrigger == true)
                {
                    hitColliders[i].GetComponent<pl_health_damage>().HandleDamage(dmgInfo);
                }
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        info.rb.useGravity = true;
        bob.enabled = true;
    }
}
