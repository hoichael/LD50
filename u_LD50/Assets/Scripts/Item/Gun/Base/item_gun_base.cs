using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_gun_base : item_base
{
    [SerializeField]
    private List<Transform> shellPosList;

    [SerializeField]
    private List<item_ammo_base> shellInfoList;

    [SerializeField]
    private int shellCapacity;

    [SerializeField]
    private float shootCD;

    private bool canShoot;

    [SerializeField]
    private List<Transform> currentPickupShells = new List<Transform>();

    [SerializeField]
    private float pickupLerpFactor;

    public void InitAmmoPickup(item_ammo_base ammoInfo)
    {
        if(shellInfoList.Count + 1 > shellCapacity)
        {
            // play sfx
            return;
        }

        HandleAmmoPickup(ammoInfo);
    }

    private void HandleAmmoPickup(item_ammo_base ammoInfo)
    {
        // add shell refs to local lists
        currentPickupShells.Add(ammoInfo.transform);
        shellInfoList.Add(ammoInfo);

        // remove phyiscs from shell
        Destroy(ammoInfo.rb);
        ammoInfo.col.enabled = false;

        ammoInfo.transform.SetParent(shellPosList[shellInfoList.Count - 1]);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && canShoot)
        {
            Shoot();
            StartCoroutine(HandleFirerate());
        }

        if (currentPickupShells.Count == 0) return;
        HandlePickupAnim();
    }

    private void Shoot()
    {

    }

    public void EjectShell()
    {

    }

    // coroutine needs to be tracked and stopped if active while weapon drop
    private IEnumerator HandleFirerate()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCD);
        canShoot = true;
    }

    private void HandlePickupAnim()
    {
        for(int i = 0; i < currentPickupShells.Count; i++)
        {
            // lerp position
            currentPickupShells[i].localPosition = Vector3.Lerp(
                currentPickupShells[i].localPosition,
                Vector3.zero,
                pickupLerpFactor * Time.deltaTime
                );

            // lerp rotation
            currentPickupShells[i].localRotation = Quaternion.Lerp(
                currentPickupShells[i].localRotation,
                Quaternion.identity,
                pickupLerpFactor * Time.deltaTime
                );

            if (currentPickupShells[i].localPosition == Vector3.zero && currentPickupShells[i].localRotation == Quaternion.identity)
            {
                currentPickupShells.RemoveAt(i);
            }
        }
    }
}
