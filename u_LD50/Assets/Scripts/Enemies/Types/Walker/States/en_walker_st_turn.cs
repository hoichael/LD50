using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_walker_st_turn : en_state_base
{
    [SerializeField]
    private float animSpeed;

    [SerializeField]
    private float duration;

    private Vector3 dirToTarget;
    Quaternion rotTarget;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(HandleDuration());
    }

    private void Update()
    {
        dirToTarget = (new Vector3(pl_state.Instance.GLOBAL_PL_TRANS_REF.position.x, 0, pl_state.Instance.GLOBAL_PL_TRANS_REF.position.z) - info.trans.position).normalized;
        rotTarget = Quaternion.LookRotation(dirToTarget);
        info.trans.rotation = Quaternion.Slerp(info.trans.rotation, rotTarget, animSpeed * Time.deltaTime);
    }

    private IEnumerator HandleDuration()
    {
        yield return new WaitForSeconds(duration);

        ChangeState("engage");
    }
}
