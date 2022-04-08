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
            Rigidbody objRB = currentObjects[i].GetComponent<Rigidbody>();
            objRB.isKinematic = false;
            objRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            currentObjects[i].GetComponentInChildren<Collider>().enabled = true;
        }
    }

    private void HandleItemsOnClose()
    {
        Collider[] hitColliders = Physics.OverlapBox(ItemColPosTrans.position, itemColHalfExtents, Quaternion.identity);

        for(int i = 0; i < hitColliders.Length; i++)
        {

            if(hitColliders[i].CompareTag("Interactable"))
            {
                // disable collider
                hitColliders[i].enabled = false;
                // "disable" rb
                Rigidbody hitRB = hitColliders[i].GetComponentInParent<Rigidbody>();
                hitRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                hitRB.isKinematic = true;
                // add object to itemContainer and currentObjects list
                if(!currentObjects.Contains(hitRB.gameObject))
                {
                    currentObjects.Add(hitRB.gameObject);
                    hitRB.transform.SetParent(itemContainer);
                }
            }
        }
    }
}
