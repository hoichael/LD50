using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_monosocket : MonoBehaviour, I_TakeItem
{
    [SerializeField]
    private nv_monolith manager;

    [SerializeField]
    private int_base intInfoRef;

    private Transform currentArtifactTrans;

    [SerializeField]
    private float animSpeed;

    [SerializeField]
    private AnimationCurve animCurve;

    private float currentAnimProgress = 1;
    private Vector3 artifactInitPos;

    [SerializeField]
    private Light currentArtifactLight;

    private float artifactInitLightIntensity;

    private bool checkForItemTaken;

    [SerializeField]
    private item_base currentArtifactInfo;

    [SerializeField]
    private Collider interactCol;

    private void Start()
    {
        // check for artifacts attached to monolith "by default" (set transform as child + currentArtifactInfo and currentArtifactLight in inspector)
        if(currentArtifactInfo != null)
        {
            currentArtifactTrans = currentArtifactInfo.transform;
            intInfoRef.takesItem = false;

            artifactInitLightIntensity = currentArtifactLight.intensity;
            currentArtifactLight.intensity = 0.5f;

            interactCol.enabled = false;

            checkForItemTaken = true;

            manager.AddArtifact();
        }
    }

    public void Init(item_base itemInfo)
    {
        currentArtifactInfo = itemInfo;
        currentArtifactTrans = currentArtifactInfo.transform;
        currentAnimProgress = 0;
        intInfoRef.takesItem = false;

        currentArtifactTrans.SetParent(this.transform);
        artifactInitPos = currentArtifactTrans.localPosition;

        currentArtifactLight = currentArtifactInfo.GetComponentInChildren<Light>();
        artifactInitLightIntensity = currentArtifactLight.intensity;

        interactCol.enabled = false;
    }

    public void Lock()
    {
        currentArtifactTrans.tag = "Untagged";
        //   currentArtifactInfo.col.enabled = false;
        interactCol.enabled = false;
    }

    private void Update()
    {
        if (currentAnimProgress != 1)
        {
            if(checkForItemTaken)
            {
                DimArtifactLight();
            }
            else
            {
                MoveItemToSocket();
            }
        }

        if (checkForItemTaken)
        {
            if(currentArtifactInfo.col.enabled == false)
            {
                checkForItemTaken = false;
                currentArtifactInfo = null;
                currentArtifactLight.intensity = artifactInitLightIntensity;
                intInfoRef.takesItem = true;
                interactCol.enabled = true;
                manager.RemoveArtifact();
            }
        }

    }

    private void MoveItemToSocket()
    {
        currentAnimProgress = Mathf.MoveTowards(currentAnimProgress, 1, animSpeed * Time.deltaTime);

        currentArtifactTrans.localPosition = Vector3.Lerp(
            artifactInitPos,
            Vector3.zero + Vector3.forward * -1,
            animCurve.Evaluate(currentAnimProgress)
            );
               
        if(currentAnimProgress == 1)
        {
            currentArtifactInfo.col.enabled = true;
            checkForItemTaken = true;
            Rigidbody rb = currentArtifactTrans.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            currentArtifactInfo.rb = rb;

            currentAnimProgress = 0; // reset currentAnimProgress for DimArtifactLight

            manager.AddArtifact();
        }
    }

    private void DimArtifactLight()
    {
        currentAnimProgress = Mathf.MoveTowards(currentAnimProgress, 1, (animSpeed * 0.4f) * Time.deltaTime);

        currentArtifactLight.intensity = Mathf.Lerp(
            artifactInitLightIntensity,
            0.5f,
            currentAnimProgress
            );
    }
}
