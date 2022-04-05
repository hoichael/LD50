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
        if (Physics.Raycast(camHolderTrans.position, camHolderTrans.forward, out hit, 3.5f, ~playerLayerMask))
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
        int_base itemInfo = GetInteractable();

        if (itemInfo == null) return;

        if(itemInfo.isItem)
        {
            itemManager.InitPickup(itemInfo.gameObject.GetComponent<int_item>());
        }

        itemInfo.Init();
    }

    private int_base GetInteractable()
    {
        RaycastHit hit;
        Physics.Raycast(camHolderTrans.position, camHolderTrans.forward, out hit, 3.5f, ~playerLayerMask);
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
