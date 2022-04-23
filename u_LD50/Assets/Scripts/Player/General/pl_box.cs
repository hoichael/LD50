using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_box : MonoBehaviour
{
    [Header("External Refs")]

    [SerializeField]
    private pl_move move;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private pl_cam_rot camRot;

    [Header("Local Refs")]

    [SerializeField]
    private Transform anchorBox;

    [SerializeField]
    private Transform anchorLid;

    [SerializeField]
    private GameObject containerObj;

    [Header("Animation Handling")]

    [SerializeField]
    private float transitionSpeed;

    [SerializeField]
    private Vector3 anchorBoxActivePos;

    [SerializeField]
    private Vector3 anchorBoxHiddenPos;

    [SerializeField]
    private Vector3 anchorLidHiddenRot;

    [SerializeField]
    private Vector3 anchorLidActiveRot;

    private List<GameObject> currentObjects = new List<GameObject>();

    private bool currentlyActive;

    public float transitionProgress;
    public float currentTransitionTarget;
    private bool transitionActive;

    [Header("Item Handling")]

    [SerializeField]
    private Transform ItemColPosTrans;

    [SerializeField]
    private Vector3 itemColHalfExtents;

    [SerializeField]
    private Transform itemContainer;

    private void Start()
    {
        anchorBox.localPosition = anchorBoxHiddenPos;
        anchorLid.localRotation = Quaternion.Euler(anchorLidHiddenRot);
        containerObj.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && pl_state.Instance.grounded && !transitionActive)
        {
            transitionActive = true;
            if(currentlyActive)
            {
                DeactivateBox();
            }
            else
            {
                ActivateBox();
            }
        }

        if (!transitionActive) return;
        HandleTransitionAnim();
    }

    private void HandleTransitionAnim()
    {
        transitionProgress = Mathf.MoveTowards(transitionProgress, currentTransitionTarget, transitionSpeed * Time.deltaTime);

        anchorBox.localPosition = Vector3.Lerp(
            anchorBoxHiddenPos, 
            anchorBoxActivePos, 
            transitionProgress);

        anchorLid.localRotation = Quaternion.Lerp(
            Quaternion.Euler(anchorLidHiddenRot), 
            Quaternion.Euler(anchorLidActiveRot), 
            transitionProgress);

        if (transitionProgress == currentTransitionTarget)
        {
            transitionActive = false;
            if(currentTransitionTarget == 0)
            {
                containerObj.SetActive(false);
            }
            else
            {
                HandleItemsOnOpen();
            }
        }
    }


    private void ActivateBox()
    {
        move.currentMult = 0;

        rb.velocity = Vector3.zero;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.isKinematic = true;

        currentTransitionTarget = 1;

        containerObj.SetActive(true);
        currentlyActive = true;

        camRot.InitBox();
    }

    private void DeactivateBox()
    {
        // add items currently "in box" to currentObjects and handle related logic
        HandleItemsOnClose();

        move.currentMult = 1;

        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        currentTransitionTarget = 0;

        currentlyActive = false;

        camRot.InitCloseBox();
    }

    private void HandleItemsOnOpen()
    {
        if (currentObjects.Count == 0) return;

        for(int i = 0; i < currentObjects.Count; i++)
        {
            // enable rb and collision
            //   Rigidbody objRB = currentObjects[i].GetComponent<Rigidbody>();
            //   objRB.isKinematic = false;
            //   objRB.collisionDetectionMode = CollisionDetectionMode.Continuous;

            item_base itemInfo = currentObjects[i].GetComponent<item_base>();

            //    currentObjects[i].GetComponentInChildren<Collider>().enabled = true;
            itemInfo.col.SetActive(true);

         //   Rigidbody rb = currentObjects[i].AddComponent<Rigidbody>();
            itemInfo.rb = currentObjects[i].AddComponent<Rigidbody>();

         //   objRB.freezeRotation = false;
        }
    }

    private void HandleItemsOnClose()
    {
        // empty the current items list. objects that are still "inside the box" will be added again in following code block
        currentObjects.Clear();

        Collider[] hitColliders = Physics.OverlapBox(ItemColPosTrans.position, itemColHalfExtents, Quaternion.identity);

        for(int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i].CompareTag("Interactable"))
            {
                // disable collider
                //   hitColliders[i].enabled = false;

                item_base itemInfo = hitColliders[i].GetComponentInParent<item_base>();

                itemInfo.col.SetActive(false);




            //    Rigidbody hitRB = hitColliders[i].GetComponentInParent<Rigidbody>();
               
             //   hitRB.freezeRotation = true;


                // add object to itemContainer and currentObjects list
                // check if already in list to prevent compound collider fuckery
                if(!currentObjects.Contains(itemInfo.gameObject))
                {
                    currentObjects.Add(itemInfo.gameObject);
                }

                itemInfo.transform.SetParent(itemContainer);

                Destroy(itemInfo.rb);

                /*
                if(!currentObjects.Contains(hitRB.gameObject))
                {
                    currentObjects.Add(hitRB.gameObject);
                    hitRB.transform.SetParent(itemContainer);
                }
                */
            }
        }
    }
}
