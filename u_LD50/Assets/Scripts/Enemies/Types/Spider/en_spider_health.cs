using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_health : en_health_base
{
    [SerializeField]
    private en_brain_base brain;

    [SerializeField]
    private en_info_base info;

    public override void HandleDamage(dmg_base dmgInfo)
    {
        base.HandleDamage(dmgInfo);

        HandleKnockback();
        brain.ChangeState("knock");

    }

    private void HandleKnockback()
    {
        info.rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        info.rb.AddForce((info.trans.position - pl_state.Instance.GLOBAL_CAM_REF.transform.position).normalized * 3f, ForceMode.Impulse);
    }

    /*
    protected override void HandleDeath(dmg_base dmgInfo)
    {
        base.HandleDeath(dmgInfo);
    }
    */
}
