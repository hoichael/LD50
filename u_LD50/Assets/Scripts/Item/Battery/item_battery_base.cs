using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_battery_base : item_base
{
    [SerializeField]
    public int totalUses;

    public int currentUse;

    [SerializeField]
    private float usePosOffset;

    public pl_D_flashlight currentAssociatedFlashlight;

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
        if (currentAssociatedFlashlight == null) return;
        //   base.Use();
        currentUse++;
        if (currentUse >= totalUses)
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

        currentAssociatedFlashlight.EjectBattery();
    }
}
