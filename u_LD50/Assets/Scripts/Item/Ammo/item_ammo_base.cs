using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_ammo_base : item_base
{
    [SerializeField]
    private string ammoType;

    [SerializeField]
    private int totalUses;

    private int currentUse;

    [SerializeField]
    private float usePosOffset;

    public item_gun_base currentAssociatedWeapon;

    [SerializeField]
    private GameObject modelDefault;

    [SerializeField]
    private GameObject modelDepleted;

    private void Start()
    {
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
        else
        {
            transform.position -= transform.up * usePosOffset;
        }
    }

    private void HandleDepletion()
    {
        modelDefault.SetActive(false);
        modelDepleted.SetActive(true);

        currentAssociatedWeapon.EjectShell();
    }
}
