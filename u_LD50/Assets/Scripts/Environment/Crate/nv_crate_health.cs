using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_crate_health : en_health_base
{
    [SerializeField]
    private nv_crate crateManager;

    protected override void HandleDeath(dmg_base dmgInfo)
    {
        base.HandleDeath(dmgInfo);
        crateManager.Init();
    }

}
