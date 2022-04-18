using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_walker_movetargets : MonoBehaviour
{
    [SerializeField]
    private en_walker_info info;

    private void Update()
    {
        /*
        for (int i = 0; i < info.targetList.Count; i++)
        {
            MoveTarget(info.targetList[i]);
        }
        */

        MoveTarget(info.infoLegL);
        MoveTarget(info.infoLegR);
    }

    private void MoveTarget(en_IKinfo IKinfo)
    {
        if (IKinfo.currentAnimProgress == 1) return;

        IKinfo.currentAnimProgress = Mathf.MoveTowards(IKinfo.currentAnimProgress, 1, IKinfo.animSpeed * Time.deltaTime);

        Vector3 lerpPos = Vector3.Lerp(
            IKinfo.lastTargetPos,
            IKinfo.currentTargetPos,
            IKinfo.animationCurve.Evaluate(IKinfo.currentAnimProgress)
            );

        if (IKinfo.oppositeLeg != null)
        {
            lerpPos.y += Mathf.Sin(IKinfo.currentAnimProgress * Mathf.PI) * IKinfo.legLiftOffset;
            if (IKinfo.currentAnimProgress == 1)
            {
                IKinfo.grounded = true;
            }
        }

        IKinfo.targetTrans.position = lerpPos;
    }
}
