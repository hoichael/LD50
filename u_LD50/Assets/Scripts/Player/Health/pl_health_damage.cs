using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health_damage : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float camTiltDmgOffset;

    [SerializeField]
    private float camTiltDmgSpeed;

    [SerializeField]
    private AnimationCurve animCurve;

    private float currentDmgAnimProgress = 1;

    [SerializeField]
    private float stunDragAmount;

    private float defaultDrag;

    [SerializeField]
    private pl_health_manager manager;

    private void Start()
    {
        defaultDrag = rb.drag;
    }

    public void HandleDamage(dmg_base dmgInfo)
    {
        if (pl_state.Instance.currentlyDead) return;

        pl_state.Instance.health -= dmgInfo.dmgAmount;

        manager.HandleHealthChange();

        if(currentDmgAnimProgress != 0) currentDmgAnimProgress = 0;
    }

    private void Update()
    {
        if (currentDmgAnimProgress == 1) return;

        currentDmgAnimProgress = Mathf.MoveTowards(currentDmgAnimProgress, 1, camTiltDmgSpeed * Time.deltaTime);

        // apply cam tilt
        pl_state.Instance.camTiltDmg = Mathf.Lerp(
            0,
            camTiltDmgOffset,
            animCurve.Evaluate(Mathf.PingPong(currentDmgAnimProgress, 0.5f))
            );
    }

    private void LateUpdate()
    {
        // apply "stun" / adjust drag
        rb.drag = Mathf.Lerp(
            defaultDrag,
            stunDragAmount,
            animCurve.Evaluate(Mathf.PingPong(currentDmgAnimProgress, 0.5f))
            );
    }
}
