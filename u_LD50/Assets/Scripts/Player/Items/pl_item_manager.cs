using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_item_manager : MonoBehaviour
{
    [Header("Refs")]

    [SerializeField]
    private pl_health_manager healthManager;

    [SerializeField]
    private Transform itemAnchor;

    [SerializeField]
    private pl_D_flashlight flashlight;

    [SerializeField]
    private Transform itemHolder;

    [SerializeField]
    private Transform camTrans;

    private GameObject currentItemObj;

    public item_base currentItemInfo;

    private Transform currentItemTrans;

    [Header("Pickup Anim")]

    [SerializeField]
    private float pickupLerpFactor;

    [SerializeField]
    private float pickupAnimSpeed;

    [SerializeField]
    private AnimationCurve pickupAnimCurve;

    private float currentAnimProgress;

    public bool currentlyInPickupAnim;

    private Vector3 pickupInitPos;

    private Quaternion pickupInitRot;

    [Header("Item Throw")]

    [SerializeField]
    private float baseCharge;

    [SerializeField]
    private float maxCharge;

    private float currentCharge;
    private bool currentlyCharging;

    [SerializeField]
    private float chargeStep;

    public void InitPickup(int_item pickupInfo)  // called from pl_interact
    {
        if(pickupInfo.itemType == "Battery" && flashlight.currentlyEnabled && flashlight.CanPickup(pickupInfo.GetComponent<item_battery_base>()))
        {
            flashlight.InitBatteryPickup(pickupInfo.GetComponent<item_battery_base>());
        }
        else if(pickupInfo.itemType == "Ammo" && currentItemInfo != null && currentItemInfo.type == "Shotgun")
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
        currentAnimProgress = 0;
        pickupInitPos = currentItemTrans.localPosition;
        pickupInitRot = currentItemTrans.localRotation;

        currentItemInfo = currentItemObj.GetComponent<item_base>();
        currentItemInfo.enabled = true;

        Destroy(currentItemInfo.rb);

        currentItemInfo.col.SetActive(false);    //    currentItemInfo.col.enabled = false;

        currentItemObj.tag = "Item";
        currentItemInfo.col.tag = "Item";

        if(pickupInfo.itemType == "Consumable")
        {
            item_con_base conInfo = currentItemObj.GetComponent<item_con_base>();
            conInfo.itemManager = this;
            conInfo.healthManager = healthManager;
        }
        // this is ugly. mb global ref in pl_state instance to itemAnchor would be better? ugly too but mb better
        else if(pickupInfo.itemType == "Prop")
        {
            currentItemObj.GetComponent<item_wep_base>().itemAnchor = itemAnchor;
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
        if(currentItemTrans == null)
        {
            currentlyInPickupAnim = false;
            return;
        }

        currentAnimProgress = Mathf.MoveTowards(currentAnimProgress, 1, pickupAnimSpeed * Time.deltaTime);
        
        // lerp position
        currentItemTrans.localPosition = Vector3.Lerp(
            pickupInitPos,
            Vector3.zero,
            pickupAnimCurve.Evaluate(currentAnimProgress)
            );

        // lerp rotation
        currentItemTrans.localRotation = Quaternion.Lerp(
            pickupInitRot,
            Quaternion.identity,
            pickupAnimCurve.Evaluate(currentAnimProgress)
            );
        
        if(currentAnimProgress == 1)
        {
            currentlyInPickupAnim = false;
        }
        
    }


    private void HandleCharge()
    {
        currentCharge += chargeStep;
        currentItemTrans.position = itemHolder.position - ((currentItemTrans.forward * (currentCharge / 15)) * 1);
    }

    private void HandleDrop()
    {
        currentItemTrans.SetParent(null);
        currentItemTrans = null;

        // this is retarded
        Rigidbody rb = currentItemObj.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        currentItemInfo.rb = rb;

        rb.AddForce(camTrans.forward * (baseCharge + currentCharge * 3.4f), ForceMode.Impulse);
        rb.AddTorque(camTrans.right * ((currentCharge - 0.2f) * 1.4f), ForceMode.Impulse);
        currentCharge = 0;

        currentItemInfo.col.SetActive(true);    //   currentItemInfo.col.enabled = true;
        currentItemObj.tag = "Interactable";
        currentItemInfo.col.tag = "Interactable";

        currentItemInfo.enabled = false;

        currentItemInfo = null;
    }

    public void GiveItem() // called from pl_interact if interaction target is item_taker. HandleDrop w/o re-enabling physics
    {
        currentItemTrans.SetParent(null);
        currentItemTrans = null;

        currentCharge = 0;

        currentItemInfo.col.SetActive(true);    //   currentItemInfo.col.enabled = true;
        currentItemObj.tag = "Interactable";
        currentItemInfo.col.tag = "Interactable";

        currentItemInfo.enabled = false;

        currentItemInfo = null;
    }
}
