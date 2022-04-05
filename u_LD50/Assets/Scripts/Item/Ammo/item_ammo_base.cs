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

    public item_gun_base currentAssociatedWeapon;

    [SerializeField]
    private GameObject modelDefault;

    [SerializeField]
    private GameObject modelDepleted;

    private void Start()
    {
        currentUse = totalUses;
        modelDefault.SetActive(true);
        modelDepleted.SetActive(false);
    }

    public override void Use()
    {
        //   base.Use();
        currentUse++;

        if(currentUse == totalUses)
        {
            HandleDepletion();
        }
        
    }

    private void HandleDepletion()
    {
        modelDefault.SetActive(false);
        modelDepleted.SetActive(true);

     //    tag.Remove(0);

        currentAssociatedWeapon.EjectShell();
    }
}
