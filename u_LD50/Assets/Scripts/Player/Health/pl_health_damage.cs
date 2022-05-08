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
    private float currentStunProgress = 1;

    [SerializeField]
    private float stunDragAmount;

    private float defaultDrag;

    [SerializeField]
    private pl_health_manager manager;

    [SerializeField]
    private pl_health_iframes iFrameManager;

    private void Start()
    {
        defaultDrag = rb.drag;
    }

    public void HandleDamage(dmg_base dmgInfo)
    {
        if (iFrameManager.enabled || pl_state.Instance.currentlyDead) return;

        iFrameManager.enabled = true;

        pl_state.Instance.health -= dmgInfo.dmgAmount;

        manager.HandleHealthChange();

        currentDmgAnimProgress = currentStunProgress = 0;
    }

    private void Update()
    {
        HandleHitAnim();
        HandleHitStun();
    }

    private void HandleHitAnim()
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

    private void HandleHitStun()
    {
        if (currentStunProgress == 1)
        {
            if (pl_state.Instance.grounded) // yes
            {
                rb.drag = pl_settings.Instance.dragGround;
            }
            else
            {
                rb.drag = pl_settings.Instance.dragAir;
            }

            return;
        }

        currentStunProgress = Mathf.MoveTowards(currentStunProgress, 1, 1.2f * Time.deltaTime);

        // apply "stun" / adjust drag
        rb.drag = Mathf.Lerp(
            stunDragAmount,
            defaultDrag,
            animCurve.Evaluate(currentStunProgress)
            );
    }
}
