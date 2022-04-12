using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health_damage : MonoBehaviour
{
    [SerializeField]
    private pl_health_ui healthUI;

    [SerializeField]
    private float camTiltDmgOffset;

    [SerializeField]
    private float camTiltDmgSpeed;

    [SerializeField]
    private AnimationCurve camTiltDmgCurve;

    private float currentDmgAnimProgress = 1;

    public void HandleDamage(dmg_base dmgInfo)
    {
        pl_state.Instance.health -= dmgInfo.dmgAmount;

        healthUI.HealthChange();

        if(currentDmgAnimProgress == 1 || currentDmgAnimProgress == 0) currentDmgAnimProgress = 0;
    }

    private void Update()
    {
        if (currentDmgAnimProgress == 1) return;

        currentDmgAnimProgress = Mathf.MoveTowards(currentDmgAnimProgress, 1, camTiltDmgSpeed * Time.deltaTime);

        pl_state.Instance.camTiltDmg = Mathf.Lerp(
            0,
            camTiltDmgOffset,
            camTiltDmgCurve.Evaluate(Mathf.PingPong(currentDmgAnimProgress, 0.5f))
            );
    }
}
