using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_con_orb : item_con_base
{
    protected override void HandleConsumption()
    {
        base.HandleConsumption();

        healthManager.savePos = pl_state.Instance.GLOBAL_PL_TRANS_REF.position;
    }
}
