using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_spider_0_legs : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private en_info_base info;

    [SerializeField]
    private Transform bodyTrans;

    [Header("Body Settings")]
    [SerializeField]
    private float groundDistance;


    [Header("Leg Info Sets")]

    [SerializeField]
    private en_spider_leg_info infoLF;

    [SerializeField]
    private en_spider_leg_info infoLB;

    [SerializeField]
    private en_spider_leg_info infoRF;

    [SerializeField]
    private en_spider_leg_info infoRB;

    private List<en_spider_leg_info> infoList; // storing info in list for easy iteration

    [Header("Leg Settings")]

    [SerializeField]
    float initialLegOffset;

    [SerializeField]
    private float legMoveTriggerDistance;

    [SerializeField]
    private float legMoveSpeed;

    [SerializeField]
    private float legLiftOffset;

    [SerializeField]
    private AnimationCurve legAnimCurve;

    [Header("Raycast Settings")]

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float raycastLegLength;

    [SerializeField]
    private float raycastBodyLength;


    private void Start()
    {
        infoLF.oppositeLeg = infoRF;
        infoLB.oppositeLeg = infoRB;
        infoRF.oppositeLeg = infoLF;
        infoRB.oppositeLeg = infoLB;

        infoList = new List<en_spider_leg_info>();
        infoList.Add(infoLF);
        infoList.Add(infoLB);
        infoList.Add(infoRF);
        infoList.Add(infoRB);

        // offset 1 leg on each side
        infoLF.targetTrans.position -= new Vector3(0, 0, initialLegOffset);
        infoRB.targetTrans.position -= new Vector3(0, 0, initialLegOffset);

        for (int i = 0; i < infoList.Count; i++)
        {
            infoList[i].currentTargetPos = infoList[i].targetTrans.position;
            infoList[i].lastTargetPos = infoList[i].targetTrans.position;
        }
    }

    private void FixedUpdate()
    {
        UpdateTargetPositions();
        MoveLegs();
     //   HandleBodyTransform();
     //   GroundCheck();
    }

    private void UpdateTargetPositions()
    {
        for (int i = 0; i < infoList.Count; i++)
        {
            // set targetObj pos to currentTargetPos value each frame to override child / parent movement
            if(infoList[i].grounded)
            {
                infoList[i].targetTrans.position = infoList[i].currentTargetPos;
            }

            RaycastHit hit;
            if(Physics.Raycast(infoList[i].raycastOrigin.position, infoList[i].raycastOrigin.up * -1, out hit, raycastLegLength, groundMask))
            {

                infoList[i].currentRayPos = hit.point;

                if (Vector3.Distance(hit.point, infoList[i].currentTargetPos) > legMoveTriggerDistance)
                {
                    if (!infoList[i].oppositeLeg.grounded) return;
                    
                    if(infoList[i].grounded)
                    {
                        infoList[i].lastTargetPos = infoList[i].currentTargetPos;
                        infoList[i].currentTargetPos = hit.point;
                        infoList[i].grounded = false;
                    }
                }
            }

        //    infoList[i].targetTrans.position = infoList[i].lastTargetPos;
        }
    }

    private void MoveLegs()
    {
        
        for (int i = 0; i < infoList.Count; i++)
        {
            if (!infoList[i].grounded)
            {
                infoList[i].currentAnimProgress = Mathf.MoveTowards(infoList[i].currentAnimProgress, 1, legMoveSpeed * Time.deltaTime);

                infoList[i].targetTrans.position = Vector3.Lerp(
                    infoList[i].lastTargetPos,
                    infoList[i].currentTargetPos,
                    legAnimCurve.Evaluate(infoList[i].currentAnimProgress)
                    );

                float currentLiftOffset = Mathf.Lerp(
                    0,
                    legLiftOffset,
                    legAnimCurve.Evaluate(Mathf.PingPong(infoList[i].currentAnimProgress, 0.5f))
                    );

                infoList[i].targetTrans.position += new Vector3(0, currentLiftOffset, 0);

                if (infoList[i].currentAnimProgress == 1)
                {
                    infoList[i].currentAnimProgress = 0;
                    infoList[i].grounded = true;
                }
            }
        }
        
    }

    // Draw DB visualizers at each current ray hit pos
    private void OnDrawGizmos()
    {
        for (int i = 0; i < infoList.Count; i++)
        {

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(infoList[i].currentRayPos, 0.5f);
        }
    }

    /*
    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(info.trans.position, info.trans.up * -1, out hit, raycastBodyLength, groundMask))
        {
            info.rb.useGravity = false;
        //    transform.position = new Vector3(transform.position.x, hit.point.y + groundDistance, transform.position.z);
        }
        else
        {
            info.rb.useGravity = true;
        }
    }

    private void HandleBodyTransform()
    {
        Vector3 avgLegPos = Vector3.zero;

        for(int i = 0; i < infoList.Count; i++)
        {
            avgLegPos += infoList[i].targetTrans.position;
        }

        avgLegPos /= 4;


    }
    */
}




[System.Serializable]
public class en_spider_leg_info
{
    public Transform targetTrans;
    public Transform raycastOrigin;
    public Vector3 lastTargetPos;
    public Vector3 currentTargetPos;
    public Vector3 currentRayPos;
    public bool grounded;
    public en_spider_leg_info oppositeLeg;
    public float currentAnimProgress;
}