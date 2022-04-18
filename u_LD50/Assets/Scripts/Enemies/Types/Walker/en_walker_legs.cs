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

    private void FixedUpdate()
    {
        UpdateTargetPositions();
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
                        info.legList[i].currentAnimProgress = 0;
                    }
                }
            }
        }
    }
}
