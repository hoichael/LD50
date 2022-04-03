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
        print("hola");
        // if currently holding item, drop item
        if(currentItemInfo != null)
        {
            HandleDrop();
        }

        GameObject itemInstance = Instantiate(pickupInfo.dynamicItemPrefab,
            itemHolder.transform);
     //   itemInstance.transform.SetParent(dynItemContainer.transform);

        currentItemInfo = itemInstance.GetComponent<item_base>();
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

        currentCharge += chargeStep;
    }

    private void HandleDrop()
    {
        GameObject dropInstance = Instantiate(currentItemInfo.pickupPrefab, itemHolder.transform.position, Quaternion.identity);

        dropInstance.GetComponent<Rigidbody>().AddForce(camTrans.forward * (baseCharge + currentCharge), ForceMode.Force);

        currentCharge = 0;
        Destroy(currentItemInfo.gameObject);
        currentItemInfo = null;
    }

}
