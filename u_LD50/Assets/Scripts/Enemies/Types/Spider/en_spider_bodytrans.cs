using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_bodytrans : MonoBehaviour
{
    [SerializeField]
    private en_spider_info info;

    [SerializeField]
    private float bodyRotMult;

    [SerializeField]
    private float bodyRotUpdateFactor;

    [SerializeField]
    private float bodyPosUpdateFactor;

    private void Update()
    {
        HandleBodyTransform();
    }

    private void HandleBodyTransform()
    {
        Vector3 avgLegPos = Vector3.zero;

        for (int i = 0; i < info.legList.Count; i++)
        {
            avgLegPos += info.legList[i].targetTrans.position;
        }

        avgLegPos /= 4;

        info.bodyTrans.rotation = Quaternion.Slerp(
            info.bodyTrans.rotation,
            GetBodyRotation(avgLegPos),
            bodyRotUpdateFactor * Time.deltaTime
            );

        return;
        info.bodyTrans.position = Vector3.MoveTowards(
        info.bodyTrans.position,
        new Vector3(
            info.bodyTrans.position.x,
            avgLegPos.y + 0.3f, // not adding offset bc body origin of current model is on same height as leg tips 
            info.bodyTrans.position.z
            ),
        bodyPosUpdateFactor * Time.deltaTime
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
