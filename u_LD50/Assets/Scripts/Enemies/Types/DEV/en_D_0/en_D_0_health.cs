using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_0_health : en_health_base
{
    [SerializeField]
    private en_brain_base brain;

    [SerializeField]
    private en_info_base info;

    protected override void HandleDeath(dmg_base dmgInfo)
    {
        base.HandleDeath(dmgInfo);

        HandleKnockback(dmgInfo);

        brain.ChangeState("dead");

        info.tag = "Corpse";
    }

    private void HandleKnockback(dmg_base dmgInfo)
    {
        // this is dumb rn but will mb be sensible later down the line
        if (dmgInfo is dmg_hitscan hitscanInfo)
        {
            ApplyKnockback(hitscanInfo.origin, dmgInfo.force);
        }
        else
        {
            ApplyKnockback(pl_state.Instance.GLOBAL_PL_TRANS_REF.position, dmgInfo.force);
        }
    }

    private void ApplyKnockback(Vector3 source, float force)
    {
        info.rb.AddForce((info.trans.position - source).normalized * force * info.knockbackMult);
    }

}