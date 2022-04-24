using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_goat_st_idle : en_state_base
{
    [SerializeField]
    private float checkInterval;

    [SerializeField]
    private float triggerDistEngage;

    [SerializeField]
    private float triggerDistAttack;

    private Transform targetObj;

    [SerializeField]
    private FMODUnity.StudioEventEmitter sfxIdleEmitter;

    private void Start()
    {
        targetObj = pl_state.Instance.GLOBAL_PL_TRANS_REF;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(CheckPlayerDist());
        info.anim.SetBool("idle", true);
        sfxIdleEmitter.Play();
    }

    private IEnumerator CheckPlayerDist()
    {
        yield return new WaitForSeconds(checkInterval);

        float dist = Vector3.Distance(info.trans.position, targetObj.position);

        if (dist < triggerDistAttack)
        {
            ChangeState("attack");
        }
        else if(dist < triggerDistEngage)
        {
            ChangeState("jump");
        }
        else
        {
            StartCoroutine(CheckPlayerDist());
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        info.anim.SetBool("idle", false);
        sfxIdleEmitter.Stop();
    }
}
