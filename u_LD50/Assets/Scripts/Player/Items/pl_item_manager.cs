using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_item_manager : MonoBehaviour
{
    [SerializeField]
    private Transform itemHolder;

    [SerializeField]
    private Transform camTrans;

    private GameObject currentItemObj;

    public item_base currentItemInfo;

    private Transform currentItemTrans;

    [SerializeField]
    private float pickupLerpFactor;

    public bool currentlyInPickupAnim;

    [SerializeField]
    private float baseCharge;

    [SerializeField]
    private float maxCharge;

    private float currentCharge;
    private bool currentlyCharging;

    [SerializeField]
    private float chargeStep;

    [SerializeField]
    private pl_health_ui healthUI;

    public void InitPickup(int_item pickupInfo)  // called from pl_interact
    {
        // if currently holding item, drop item
        if(pickupInfo.itemType == "Ammo" && currentItemInfo != null && currentItemInfo.type == "Shotgun")
        {
            currentItemInfo.GetComponent<item_gun_base>().InitAmmoPickup(pickupInfo.GetComponent<item_ammo_base>());
        }
        else
        {
            if (currentItemInfo != null)
            {
                HandleDrop();
            }

            HandlePickup(pickupInfo);
        }
    }

    private void HandlePickup(int_item pickupInfo)
    {
        currentItemObj = pickupInfo.gameObject;

        currentItemTrans = currentItemObj.transform;

        currentItemTrans.SetParent(itemHolder);
        currentItemTrans.localScale = new Vector3(1, 1, 1);
        
     //    currentItemTrans.localPosition = Vector3.zero;
     //    currentItemTrans.localRotation = Quaternion.identity;
        
        currentlyInPickupAnim = true;

        currentItemInfo = currentItemObj.GetComponent<item_base>();
        currentItemInfo.enabled = true;

        Destroy(currentItemInfo.rb);

        currentItemInfo.col.enabled = false;

        currentItemObj.tag = "Item";
        currentItemInfo.col.tag = "Item";

        if(pickupInfo.itemType == "Consumable")
        {
            item_con_base conInfo = currentItemObj.GetComponent<item_con_base>();
            conInfo.itemManager = this;
            conInfo.healthUI = healthUI;
        }
    }

    private void Update()
    {
        if(currentlyInPickupAnim)
        {
            HandlePickupAnim();
        }

        if(currentItemInfo != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                currentlyInPickupAnim = false;
                currentItemTrans.localPosition = Vector3.zero;
                currentItemTrans.localRotation = Quaternion.identity;

                currentlyCharging = true;
            }

            if (Input.GetMouseButtonDown(0) && currentItemInfo.type != "Ammo")
            {
                currentItemInfo.Use();
            }
        }

        if (Input.GetMouseButtonUp(1) && currentlyCharging)
        {
            currentlyCharging = false;
            HandleDrop();
        }
    }

    private void FixedUpdate()
    {
        if (!currentlyCharging || currentCharge > maxCharge) return;

        HandleCharge();
    }

    private void HandlePickupAnim()
    {
        
        // lerp position
        currentItemTrans.localPosition = Vector3.Lerp(
            currentItemTrans.localPosition,
            Vector3.zero,
            pickupLerpFactor * Time.deltaTime
            );
        /*
        currentItemTrans.localPosition = Vector3.MoveTowards(
            currentItemTrans.localPosition,
            Vector3.zero,
            pickupLerpFactor * Time.deltaTime
            );
        */
        // lerp rotation
        currentItemTrans.localRotation = Quaternion.Lerp(
            currentItemTrans.localRotation,
            Quaternion.identity,
            pickupLerpFactor * Time.deltaTime
            );
        
        if(currentItemTrans.localPosition == Vector3.zero && currentItemTrans.localRotation == Quaternion.identity)
        {
            currentlyInPickupAnim = false;
        }
        
    }


    private void HandleCharge()
    {
        currentCharge += chargeStep;
        currentItemTrans.position = itemHolder.position - ((currentItemTrans.forward * (currentCharge / 1600)) * 1);
    }

    private void HandleDrop()
    {
        currentItemTrans.SetParent(null);

        // this is retarded
        Rigidbody rb = currentItemObj.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        currentItemInfo.rb = rb;

        rb.AddForce(camTrans.forward * (baseCharge + currentCharge), ForceMode.Force);
        currentCharge = 0;

        currentItemInfo.col.enabled = true;
        currentItemObj.tag = "Interactable";
        currentItemInfo.col.tag = "Interactable";

        currentItemInfo.enabled = false;

        currentItemInfo = null;
    }
}
