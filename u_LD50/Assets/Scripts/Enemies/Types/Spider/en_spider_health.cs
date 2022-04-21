using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_health : en_health_base
{
    [SerializeField]
    private en_brain_base brain;

    public override void HandleDamage(dmg_base dmgInfo)
    {
        base.HandleDamage(dmgInfo);

        brain.ChangeState("knock");

    }

    /*
    protected override void HandleDeath(dmg_base dmgInfo)
    {
        base.HandleDeath(dmgInfo);
    }
    */
}
