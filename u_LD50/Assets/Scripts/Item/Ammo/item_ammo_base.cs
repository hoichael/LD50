using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_ammo_base : item_base
{
    [SerializeField]
    private string ammoType;

    public int totalUses;
    public int currentUse;

    [SerializeField]
    private float usePosOffset;

    private void Start()
    {
        currentUse = totalUses;
    }

    private void HandleDepletion()
    {

    }
}
