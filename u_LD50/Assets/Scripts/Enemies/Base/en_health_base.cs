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

    public virtual void HandleDamage(dmg_base dmgInfo)
    {
        hpCurrent -= dmgInfo.dmgAmount;
        print("received damage (" + dmgInfo.dmgAmount + ") remaining hp: " + hpCurrent);

        if (hpCurrent <= 0) HandleDeath(dmgInfo);
    }

    protected virtual void HandleDeath(dmg_base dmgInfo)
    {
        print("ded lel");
    }
}
