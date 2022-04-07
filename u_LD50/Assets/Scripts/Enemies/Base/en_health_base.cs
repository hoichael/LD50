using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_health_base : MonoBehaviour
{
    [SerializeField]
    protected int hpMax;

    [SerializeField]
    protected int hpCurrent;

    private void Start()
    {
        hpCurrent = hpMax;
    }

    public virtual void HandleDamage(dmg_base damageInfo)
    {
        hpCurrent -= damageInfo.dmgAmount;

        if (hpCurrent <= 0) HandleDeath();
    }

    protected virtual void HandleDeath()
    {
        //    print("ded lel");
    }
}
