using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class pl_interact : MonoBehaviour
{
    [SerializeField]
    private pl_item_manager itemManager;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private Transform camHolderTrans;

    [SerializeField]
    private RectTransform retTrans;

    [SerializeField]
    private float retTriggerSize;

    [SerializeField]
    private float retResizeFactor;

    private float retCurrentSize;
    private float retDefaultSize;
    private float currentRetLerpTarget;

    [SerializeField]
    private FMODUnity.EventReference fEventPickup;

    private void Start()
    {
        retDefaultSize = retCurrentSize = currentRetLerpTarget = retTrans.sizeDelta.x;
    }

    void Update()
    {
        CheckForInteractable();
        if (retCurrentSize == currentRetLerpTarget) return;
        ResizeReticle();

        if(Input.GetMouseButtonDown(2) && retCurrentSize != retDefaultSize)
        {
            InitInteraction();
        }
    }

    private bool CheckForInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(camHolderTrans.position, camHolderTrans.forward, out hit, 5.1f, ~playerLayerMask))
        {
            if(hit.transform.gameObject.CompareTag("Interactable"))
            {
                currentRetLerpTarget = retTriggerSize;
                return true;
            }
        }
        currentRetLerpTarget = retDefaultSize;
        return false;
    }

    private void InitInteraction()
    {
        int_base interactInfo = GetInteractable();

        if (interactInfo == null) return;

        if(interactInfo.isItem)
        {
            itemManager.InitPickup(interactInfo.gameObject.GetComponent<int_item>());
            FMODUnity.RuntimeManager.PlayOneShot(fEventPickup);
        }
        else if(interactInfo.takesItem)
        {
            if (itemManager.currentItemInfo == null) return;

            if (interactInfo.isMonolith && itemManager.currentItemInfo.ID != "artifact") return;

            if (interactInfo.isMinilith)
            {
                if(itemManager.currentItemInfo.ID != "glowstone_A" && itemManager.currentItemInfo.ID != "glowstone_B")
                {
                    return;
                }
            }

            item_base itemInfo = itemManager.currentItemInfo;
            itemManager.GiveItem();
            interactInfo.GetComponent<I_TakeItem>().Init(itemInfo);
        }

    //    itemInteractInfo.Init(); doesnt do anything atm
    }

    private int_base GetInteractable()
    {
        RaycastHit hit;
        Physics.Raycast(camHolderTrans.position, camHolderTrans.forward, out hit, 5.1f, ~playerLayerMask);
        if (hit.transform == null) return null;

        if(hit.transform.gameObject.CompareTag("Interactable"))
        {
            return hit.transform.gameObject.GetComponent<int_base>();
        }
        return null;
    }

    private void ResizeReticle()
    {
        retCurrentSize = Mathf.Lerp(
            retCurrentSize,
            currentRetLerpTarget,
            retResizeFactor * Time.deltaTime
            );

        retTrans.sizeDelta = new Vector2(retCurrentSize, retCurrentSize);
    }
}
