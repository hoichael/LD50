using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_swiper_attack : en_state_base
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private Transform attackPosTrans;

    [SerializeField]
    private Vector3 attackColHalfExtents;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private dmg_base dmgInfo;

    protected override void OnEnable()
    {
        base.OnEnable();

        info.anim.SetBool("attacking", true);

        StartCoroutine(HandleDuration());
    }

    private IEnumerator HandleDuration()
    {
        yield return new WaitForSeconds(duration * 0.5f);

        HandleHitbox();

        yield return new WaitForSeconds(duration * 0.5f);

        ChangeState("chase");
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

        info.anim.SetBool("attacking", false);
    }
}
