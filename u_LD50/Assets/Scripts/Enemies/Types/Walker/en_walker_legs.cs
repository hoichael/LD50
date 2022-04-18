using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_walker_legs : MonoBehaviour
{

    [SerializeField]
    private en_walker_info info;

    [SerializeField]
    private float raycastLegLength;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float legMoveTriggerDistance;

    [SerializeField]
    private float legMoveSpeed;

    private void FixedUpdate()
    {
        UpdateTargetPositions();
        MoveLegs();
        //    HandleBodyTransform();
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
            if (Physics.Raycast(info.legList[i].raycastOrigin.position, Vector3.down, out hit, raycastLegLength, groundMask))
            {
                if (Vector3.Distance(hit.point, info.legList[i].currentTargetPos) > legMoveTriggerDistance)
                {
                    if(!info.legList[i].grounded)
                    {
                        info.legList[i].currentTargetPos = hit.point;
                    }

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
             //       legAnimCurve.Evaluate(info.legList[i].currentAnimProgress)
                    info.legList[i].currentAnimProgress
                    );

                /*
                float currentLiftOffset = Mathf.Lerp(
                    0,
                    legLiftOffset,
                    legAnimCurve.Evaluate(Mathf.PingPong(info.legList[i].currentAnimProgress, 0.5f))
                    );

                info.legList[i].targetTrans.position += new Vector3(0, currentLiftOffset, 0);

                                */

                if (info.legList[i].currentAnimProgress == 1)
                {
                    info.legList[i].currentAnimProgress = 0;
                    info.legList[i].grounded = true;
                }
            }
        }
    }
}
