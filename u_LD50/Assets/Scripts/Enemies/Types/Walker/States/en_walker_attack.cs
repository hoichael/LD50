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
    private float animRotOffsetX;

    [SerializeField]
    private AnimationCurve animCurve;

    private float currentRotOffsetX;

    private float currentAnimProgress;

    private int currentLerpTarget;

    [SerializeField]
    private Transform attackPosTrans;

    [SerializeField]
    private Vector3 attackColHalfExtents;

    [SerializeField]
    private LayerMask playerLayerMask;

    protected override void OnEnable()
    {
        base.OnEnable();

        currentAnimProgress = 0;
        currentLerpTarget = 1;
        info.rb.useGravity = false;


    }

    private void Update()
    {
        
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
    }
}
