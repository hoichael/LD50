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

    private bool canShoot = true;

    [SerializeField]
    private List<Transform> currentPickupShells = new List<Transform>();

    [SerializeField]
    private float pickupLerpFactor;

    [SerializeField]
    private item_gun_recoil recoil;

    [SerializeField]
    private item_gun_flash flash;

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

        ammoInfo.currentAssociatedWeapon = this;
    }

    private void Update()
    {
        if (currentPickupShells.Count == 0) return;
        HandlePickupAnim();
    }

    public override void Use()
    {
        base.Use();
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Shoot();
            StartCoroutine(HandleFirerate());
        }
    }

    private void Shoot()
    {
        if (shellInfoList.Count == 0) return;

        if(currentPickupShells.Contains(shellInfoList[shellInfoList.Count - 1].transform))
        {
            currentPickupShells.Remove(shellInfoList[shellInfoList.Count - 1].transform);
            shellInfoList[shellInfoList.Count - 1].transform.localPosition = Vector3.zero;
            shellInfoList[shellInfoList.Count - 1].transform.localRotation = Quaternion.identity;
        }

        shellInfoList[shellInfoList.Count - 1].Use();

        recoil.Init();
        flash.Init();
    }

    public void EjectShell()
    {
        item_ammo_base shellInfo = shellInfoList[shellInfoList.Count - 1];

        shellInfoList.Remove(shellInfo);

        shellInfo.transform.SetParent(null);

        Rigidbody rb = shellInfo.gameObject.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        shellInfo.rb = rb;

        rb.AddForce(Vector3.up * 200, ForceMode.Force);
        rb.AddTorque(new Vector3(Random.Range(-0.9f, 0.9f), 0, Random.Range(-0.9f, 0.9f)) * 150);

        shellInfo.type = "Prop";
        shellInfo.tag = "Interactable";

        // this is ugly
        StartCoroutine(EnableEjectedShellCol(shellInfo.col));
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

    private IEnumerator EnableEjectedShellCol(Collider col)
    {
        yield return new WaitForSeconds(0.2f);
        col.enabled = true;
    }

    private void OnEnable()
    {
        recoil.enabled = true;
    }

    private void OnDisable()
    {
        recoil.enabled = false;
    }
}
