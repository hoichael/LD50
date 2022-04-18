using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_walker_health : en_health_base
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float stunDragAmount;

    [SerializeField]
    private float currentStunProgress;

    [SerializeField]
    private float stunSpeed;

    protected override void Start()
    {
        currentStunProgress = 1;
    }

    public override void HandleDamage(dmg_base dmgInfo)
    {
        print("DMG");
        base.HandleDamage(dmgInfo);
        currentStunProgress = 0;
    }

    private void Update()
    {
        if (currentStunProgress == 1) return;
        HandleStun();
    }

    private void HandleStun()
    {
        currentStunProgress = Mathf.MoveTowards(currentStunProgress, 1, stunSpeed * Time.deltaTime);

        rb.drag = Mathf.Lerp(
            0,
            stunDragAmount,
            Mathf.PingPong(currentStunProgress, 0.5f)
            );
    }
}
