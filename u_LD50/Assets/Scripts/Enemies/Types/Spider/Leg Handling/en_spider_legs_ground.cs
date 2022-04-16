using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_legs_ground : MonoBehaviour
{
    [Header("Refs")]

    [SerializeField]
    private en_spider_info info;

    [SerializeField]
    private Transform bodyTrans;

    [SerializeField]
    private LayerMask groundMask;

    [Header("Settings")]

    [SerializeField]
    private float raycastLegLength;

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
    private float legNormalMinY;

    [SerializeField]
    private float bodyRotMult; // mult for mapping leg target Y positions to body rotation

    [SerializeField]
    private float bodyRotUpdateFactor;

    [SerializeField]
    private float bodyPosUpdateFactor;

    [SerializeField]
    private GameObject breakDB;

    private void OnEnable()
    {
        
        for (int i = 0; i < info.legList.Count; i++)
        {
            info.legList[i].targetTrans.position = info.legList[i].targetDefaultTrans.position;

            info.legLF.targetTrans.forward -= new Vector3(0, 0, initialLegOffset);
            info.legRB.targetTrans.forward -= new Vector3(0, 0, initialLegOffset);

            info.legList[i].currentTargetPos = info.legList[i].targetTrans.position;
            info.legList[i].lastTargetPos = info.legList[i].targetTrans.position;

            info.legList[i].currentAnimProgress = 0;

            info.legList[i].grounded = true;
        }

    }

    private void FixedUpdate()
    {
        UpdateTargetPositions();
        MoveLegs();
        HandleBodyTransform();

     //   Instantiate(breakDB);
    }

    private void UpdateTargetPositions()
    {
        for (int i = 0; i < info.legList.Count; i++)
        {
            // set targetObj pos to currentTargetPos value each frame to override child / parent movement
            if (info.legList[i].grounded)
            {
                info.legList[i].targetTrans.position = info.legList[i].currentTargetPos;
            }

            RaycastHit hit;
            if (Physics.Raycast(info.legList[i].raycastOrigin.position, /* infoList[i].raycastOrigin.up */ Vector3.up * -1, out hit, raycastLegLength, groundMask))
            {

                info.legList[i].currentRayPos = hit.point;

                if (hit.normal.y > legNormalMinY) // Check if hit on very steep slope
                {
                    info.legList[i].currentRayNormal = hit.normal;

                    if (Vector3.Distance(hit.point, info.legList[i].currentTargetPos) > legMoveTriggerDistance)
                    {
                        if (!info.legList[i].oppositeLeg.grounded) return;

                        if (info.legList[i].grounded)
                        {
                            info.legList[i].lastTargetPos = info.legList[i].currentTargetPos;
                            info.legList[i].currentTargetPos = hit.point;
                            info.legList[i].grounded = false;
                        }
                    }
                }
            }
        }
    }

    private void MoveLegs()
    {

        for (int i = 0; i < info.legList.Count; i++)
        {
            if (!info.legList[i].grounded)
            {
                info.legList[i].currentAnimProgress = Mathf.MoveTowards(info.legList[i].currentAnimProgress, 1, legMoveSpeed * Time.deltaTime);

                info.legList[i].targetTrans.position = Vector3.Lerp(
                    info.legList[i].lastTargetPos,
                    info.legList[i].currentTargetPos,
                    legAnimCurve.Evaluate(info.legList[i].currentAnimProgress)
                    );

                float currentLiftOffset = Mathf.Lerp(
                    0,
                    legLiftOffset,
                    legAnimCurve.Evaluate(Mathf.PingPong(info.legList[i].currentAnimProgress, 0.5f))
                    );

                info.legList[i].targetTrans.position += new Vector3(0, currentLiftOffset, 0);

                if (info.legList[i].currentAnimProgress == 1)
                {
                    info.legList[i].currentAnimProgress = 0;
                    info.legList[i].grounded = true;
                }
            }
        }

    }

    private void HandleBodyTransform()
    {
        Vector3 avgLegPos = Vector3.zero;

        for (int i = 0; i < info.legList.Count; i++)
        {
            avgLegPos += info.legList[i].targetTrans.position;
        }

        avgLegPos /= 4;


        Vector3 avgLegNormal = Vector3.zero;

        for (int i = 0; i < info.legList.Count; i++)
        {
            avgLegNormal += info.legList[i].currentRayNormal;
        }

        avgLegNormal /= 4;


        // INTERPOLATE position
        bodyTrans.position = Vector3.MoveTowards(
            bodyTrans.position,
            new Vector3(
                bodyTrans.position.x,
                avgLegPos.y - 0.6f, // not adding offset bc body origin of current model is on same height as leg tips 
                bodyTrans.position.z
                ),
            bodyPosUpdateFactor * Time.fixedDeltaTime
            );

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
        offset = info.legLF.targetTrans.position.y - avgLegPos.y;
        offsetLF = new Vector3(offset * -bodyRotMult, 0, offset * -bodyRotMult);

        // handle LB
        offset = info.legLB.targetTrans.position.y - avgLegPos.y;
        offsetLB = new Vector3(offset * bodyRotMult, 0, offset * -bodyRotMult);

        // handle RF
        offset = info.legRF.targetTrans.position.y - avgLegPos.y;
        offsetRF = new Vector3(offset * -bodyRotMult, 0, offset * bodyRotMult);

        // handle RB
        offset = info.legRB.targetTrans.position.y - avgLegPos.y;
        offsetRB = new Vector3(offset * bodyRotMult, 0, offset * bodyRotMult);

        // calc final offset
        Vector3 finalOffset = (offsetLF + offsetLB + offsetRF + offsetRB) / 4;

        // convert to quaternion
        return Quaternion.Euler(finalOffset);
    }
}
