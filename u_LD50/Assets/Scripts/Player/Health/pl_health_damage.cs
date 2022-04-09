using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health_damage : MonoBehaviour
{
    [SerializeField]
    private pl_health_ui healthUI;

    public void HandleDamage(dmg_base dmgInfo)
    {
        pl_state.Instance.health -= dmgInfo.dmgAmount;

        healthUI.HealthChange();
    }
}
