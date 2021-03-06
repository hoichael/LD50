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

    [SerializeField]
    private float bodyRotMult; // mult for mapping leg target Y positions to body rotation

    [SerializeField]
    private float bodyRotUpdateFactor;

    [SerializeField]
    private float bodyPosUpdateFactor;

    [Header("Leg Info Sets")]

    [SerializeField]
    private en_spider_leg_info infoLF;

    [SerializeField]
    private en_spider_leg_info infoLB;

    [SerializeField]
    private en_spider_leg_info infoRF;

    [SerializeField]
    private en_spider_leg_info infoRB;

    private List<en_spider_leg_info> infoList = new List<en_spider_leg_info>(); // storing info in list for easy iteration

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

    [SerializeField]
    [Range(0f, 1f)]
    private float legNormalMinY; // used to not set target pos to very steep slopes

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
        GroundCheck();

        UpdateTargetPositions();
        MoveLegs();
        HandleBodyTransform();
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
            if(Physics.Raycast(infoList[i].raycastOrigin.position, /* infoList[i].raycastOrigin.up */ Vector3.up * -1, out hit, raycastLegLength, groundMask))
            {

                infoList[i].currentRayPos = hit.point;

                if(hit.normal.y > legNormalMinY) // Check if hit on very steep slope
                {
                    infoList[i].currentRayNormal = hit.normal;

                    if (Vector3.Distance(hit.point, infoList[i].currentTargetPos) > legMoveTriggerDistance)
                    {
                        if (!infoList[i].oppositeLeg.grounded) return;

                        if (infoList[i].grounded)
                        {
                            infoList[i].lastTargetPos = infoList[i].currentTargetPos;
                            infoList[i].currentTargetPos = hit.point;
                            infoList[i].grounded = false;
                        }
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

    private void GroundCheck()
    {
        print("a");
        RaycastHit hit;
        if (Physics.Raycast(bodyTrans.position + Vector3.up * 1.2f, Vector3.up * -1, out hit, raycastBodyLength, groundMask))
        {
            print("b");
            info.rb.useGravity = false;
            info.grounded = true;
        //    transform.position = new Vector3(transform.position.x, hit.point.y + groundDistance, transform.position.z);
        }
        else
        {
            info.rb.useGravity = true;
            info.grounded = false;
        }
    }

 
    private void HandleBodyTransform()
    {
        Vector3 avgLegPos = Vector3.zero;

        for(int i = 0; i < infoList.Count; i++)
        {
               avgLegPos += infoList[i].targetTrans.position;
        //    avgLegPos += infoList[i].currentTargetPos;
        }

        avgLegPos /= 4;


        Vector3 avgLegNormal = Vector3.zero;

        for (int i = 0; i < infoList.Count; i++)
        {
            avgLegNormal += infoList[i].currentRayNormal;

            // DB DB DB DB DB DB DB DB DB DB DB DB DB DB DB DB DB DB
        //    print("current ray normal: " + infoList[i].currentRayNormal);
        }

        avgLegNormal /= 4;

        /*
        // SET position
        bodyTrans.position = new Vector3(
            bodyTrans.position.x,
            avgLegPos.y, // not adding offset bc body origin of current model is on same height as leg tips 
            bodyTrans.position.z
            );

        // SET rotation
        //   bodyTrans.rotation = Quaternion.FromToRotation(bodyTrans.up, avgLegNormal) * bodyTrans.rotation;
        bodyTrans.up = avgLegNormal;
        */

        
        // INTERPOLATE position
        bodyTrans.position = Vector3.MoveTowards(
            bodyTrans.position,
            new Vector3(
                bodyTrans.position.x,
                avgLegPos.y, // not adding offset bc body origin of current model is on same height as leg tips 
                bodyTrans.position.z
                ),
            bodyPosUpdateFactor * Time.fixedDeltaTime
            );
        


        // INTERPOLATE rotation
        /*
        bodyTrans.up = Vector3.MoveTowards(
            bodyTrans.up,
            avgLegNormal,
            bodyRotUpdateFactor * Time.fixedDeltaTime
            );
        */
        bodyTrans.rotation = Quaternion.Slerp(
            bodyTrans.rotation,
            GetBodyRotation(avgLegPos),
            bodyRotUpdateFactor * Time.fixedDeltaTime
            );
    }

    private Quaternion GetBodyRotation(Vector3 avgLegPos)
    {
        float offset;
        Vector3 offsetLF, offsetLB, offsetRF, offsetRB;

        // handle LF
        offset = infoLF.targetTrans.position.y - avgLegPos.y;
        offsetLF = new Vector3(offset * -bodyRotMult, 0, offset * -bodyRotMult);

        // handle LB
        offset = infoLB.targetTrans.position.y - avgLegPos.y;
        offsetLB = new Vector3(offset * bodyRotMult, 0, offset * -bodyRotMult);

        // handle RF
        offset = infoRF.targetTrans.position.y - avgLegPos.y;
        offsetRF = new Vector3(offset * -bodyRotMult, 0, offset * bodyRotMult);

        // handle RB
        offset = infoRB.targetTrans.position.y - avgLegPos.y;
        offsetRB = new Vector3(offset * bodyRotMult, 0, offset * bodyRotMult);

        // calc final offset
        Vector3 finalOffset = (offsetLF + offsetLB + offsetRF + offsetRB) / 4;

        // convert to quaternion
        return Quaternion.Euler(finalOffset);
    }
}


/*
[System.Serializable]
public class en_spider_leg_info
{
    public Transform targetTrans;
    public Transform raycastOrigin;
    public Vector3 lastTargetPos;
    public Vector3 currentTargetPos;
    public Vector3 currentRayPos;
    public Vector3 currentRayNormal;
    public bool grounded;
    public en_spider_leg_info oppositeLeg;
    public float currentAnimProgress;
}
*/