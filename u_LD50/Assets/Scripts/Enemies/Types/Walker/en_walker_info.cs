using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_walker_info : en_info_base
{
    [Header("IK Info Atlas")]

    public en_IKinfo infoLegL;

    public en_IKinfo infoLegR;

    public en_IKinfo infoArmL;

    public en_IKinfo infoArmR;

    public en_IKinfo infoHead;

    public en_IKinfo infoChest;

    public List<en_IKinfo> legList = new List<en_IKinfo>();

    public List<en_IKinfo> targetList = new List<en_IKinfo>();

    private void Awake()
    {
        infoLegL.oppositeLeg = infoLegR;
        infoLegR.oppositeLeg = infoLegL;

        legList.Add(infoLegL);
        legList.Add(infoLegR);

        infoLegL.targetTrans.position -= new Vector3(0, 0, 0.4f);

        targetList.Add(infoLegL);
        targetList.Add(infoLegR);
        targetList.Add(infoArmL);
        targetList.Add(infoArmR);
        targetList.Add(infoHead);
        targetList.Add(infoChest);

        for (int i = 0; i < targetList.Count; i++)
        {
            targetList[i].currentTargetPos = targetList[i].lastTargetPos = targetList[i].targetTrans.position;
            targetList[i].currentAnimProgress = 1;
        }
    }
}
