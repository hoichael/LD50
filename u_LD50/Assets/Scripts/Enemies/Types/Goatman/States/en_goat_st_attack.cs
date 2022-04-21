using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_goat_st_attack : en_state_base
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private float turnSpeed;
    private Transform targetTrans;
    private Vector3 dirToTarget;
    private Quaternion targetRot;

    [SerializeField]
    private dmg_base dmgInfo;

    [SerializeField]
    private Transform attackPosTrans;

    [SerializeField]
    private Vector3 attackColHalfExtents;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private float dmgInterval;

    [SerializeField]
    private int attackAmount;

    private int attackCounter;

    private void Start()
    {
        targetTrans = pl_state.Instance.GLOBAL_PL_TRANS_REF;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        attackCounter = 0;

        info.anim.SetBool("attack", true);
        StartCoroutine(HandleDuration());

        StartCoroutine(DamageRoutine());
    }

    private void Update()
    {
        dirToTarget = (
            new Vector3(targetTrans.position.x, info.trans.position.y, targetTrans.position.z)
            - info.trans.position).normalized;

        targetRot = Quaternion.LookRotation(dirToTarget);
        info.trans.rotation = Quaternion.Slerp(info.trans.rotation, targetRot, turnSpeed * Time.deltaTime);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        info.anim.SetBool("attack", false);
    }

    private IEnumerator DamageRoutine()
    {
        yield return new WaitForSeconds(dmgInterval);

        attackCounter++;

        HandleHitbox();

        if(attackCounter < attackAmount)
        {
            StartCoroutine(DamageRoutine());
        }
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

    private IEnumerator HandleDuration()
    {
        yield return new WaitForSeconds(duration);

        ChangeState("idle");
    }
}
