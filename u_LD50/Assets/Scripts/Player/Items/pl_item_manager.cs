using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_item_manager : MonoBehaviour
{
    [SerializeField]
    private GameObject dynItemContainer;

    [SerializeField]
    private Transform orientation;

    private item_base currentItemInfo;

    private float currentCharge;
    private bool currentlyCharging;

    [SerializeField]
    private float chargeStep;

    public void Pickup(item_base itemInfo)  // called from pl_interact
    {
        // if currently holding item, drop item
        if(currentItemInfo != null)
        {
            HandleDrop();
        }

        currentItemInfo = itemInfo;

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1) && currentItemInfo != null)
        {
            currentlyCharging = true;
        }

        if (Input.GetMouseButtonUp(1) && currentlyCharging)
        {
            HandleDrop();
        }
    }

    private void FixedUpdate()
    {
        if (!currentlyCharging) return;

        currentCharge += chargeStep;
    }

    private void HandleDrop()
    {
        GameObject dropInstance = Instantiate(currentItemInfo.pickupPrefab, transform.position, Quaternion.identity);
        dropInstance.GetComponent<Rigidbody>().AddForce(orientation.forward * currentCharge, ForceMode.Force);

        Destroy(currentItemInfo.gameObject);
    }

}
