using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_box : MonoBehaviour
{
    [SerializeField]
    private pl_move move;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Transform anchorBox;

    [SerializeField]
    private Transform anchorLid;

    [SerializeField]
    private GameObject containerObj;

    [SerializeField]
    private pl_cam_rot camRot;

    [SerializeField]
    private Vector3 anchorBoxActivePos;

    [SerializeField]
    private Vector3 anchorBoxHiddenPos;

    [SerializeField]
    private Vector3 anchorLidHiddenRot;

    [SerializeField]
    private Vector3 anchorLidActiveRot;

    private List<GameObject> currentObjects;

    private bool currentlyActive;

    [SerializeField]
    private float transitionSpeed;

    public float transitionProgress;
    public float currentTransitionTarget;
    private bool transitionActive;

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
        move.currentMult = 1;

        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        currentTransitionTarget = 0;

        currentlyActive = false;

        camRot.InitCloseBox();
    }
}
