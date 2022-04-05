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
    private float shootCD;

    private bool canShoot;

    public void HandleAmmoPickup()
    {

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && canShoot)
        {
            Shoot();
            StartCoroutine(HandleFirerate());
        }
    }

    private void Shoot()
    {

    }

    // coroutine needs to be tracked and stopped if active while weapon drop
    private IEnumerator HandleFirerate()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCD);
        canShoot = true;
    }
}
