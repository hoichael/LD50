using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_swiper_attack : en_state_base
{
    [SerializeField]
    private float duration;

    protected override void OnEnable()
    {
        base.OnEnable();

        info.anim.SetBool("attacking", true);

        StartCoroutine(HandleDuration());
    }

    private IEnumerator HandleDuration()
    {
        yield return new WaitForSeconds(duration);

        ChangeState("chase");
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        info.anim.SetBool("attacking", false);
    }
}
