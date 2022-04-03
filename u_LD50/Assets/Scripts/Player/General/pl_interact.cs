using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class pl_interact : MonoBehaviour
{
    [SerializeField]
    private pl_item_manager itemManager;

    [SerializeField]
    private Transform camTrans;

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
    }

    private bool CheckForInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 5f))
        {
            if(hit.transform.gameObject.CompareTag("Interactable"))
            {
                print("YO");
                currentRetLerpTarget = retTriggerSize;
                return true;
            }
        }
        currentRetLerpTarget = retDefaultSize;
        return false;
    }

    private void InitInteraction()
    {
    //    itemManager.Pickup(GetInteractable());
        GetInteractable().Init();
    }

    private int_base GetInteractable()
    {
        RaycastHit hit;
        Physics.Raycast(camTrans.position, camTrans.forward, out hit, 10f);
        return hit.transform.gameObject.GetComponent<int_base>();
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
