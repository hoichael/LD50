using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_item_manager : MonoBehaviour
{
    [SerializeField]
    private GameObject itemHolder;

    [SerializeField]
    private Transform camTrans;

    private item_base currentItemInfo;

    private Transform currentItemTrans;

    private GameObject currentModel;

    [SerializeField]
    private float baseCharge;

    [SerializeField]
    private float maxCharge;

    private float currentCharge;
    private bool currentlyCharging;

    [SerializeField]
    private float chargeStep;

    public void Pickup(int_item pickupInfo)  // called from pl_interact
    {
        // if currently holding item, drop item
        if(currentItemInfo != null)
        {
            HandleDrop();
        }

        InstantiatePickedUpObj(pickupInfo);

    }

    private void InstantiatePickedUpObj(int_item pickupInfo)
    {
        // spawn parent object
        GameObject itemInstance = Instantiate(pickupInfo.itemParentPrefab,
        itemHolder.transform);

        // set item values
        currentItemInfo = itemInstance.GetComponent<item_base>();
        currentItemTrans = itemInstance.transform;

        // handle child object containing model
        currentModel = pickupInfo.modelObj;
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.SetParent(currentItemTrans);
        currentModel.tag = "Item";
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1) && currentItemInfo != null)
        {
            currentlyCharging = true;
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

    private void HandleCharge()
    {
        currentCharge += chargeStep;
        currentItemTrans.position = itemHolder.transform.position - ((currentItemTrans.forward * (currentCharge / 1600)) * 1);
    }

    private void HandleDrop()
    {
        GameObject dropInstance = Instantiate(currentItemInfo.pickupPrefab, currentItemTrans.position, Quaternion.identity);

        dropInstance.GetComponent<Rigidbody>().AddForce(camTrans.forward * (baseCharge + currentCharge), ForceMode.Force);

        currentCharge = 0;
        Destroy(currentItemInfo.gameObject);
        currentItemInfo = null;





        //         currentModel.tag = "Interactable";
    }

}
