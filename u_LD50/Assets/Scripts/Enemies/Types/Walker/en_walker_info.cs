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


    private void Awake()
    {
        infoLegL.oppositeLeg = infoLegR;
        infoLegR.oppositeLeg = infoLegL;

        legList.Add(infoLegL);
        legList.Add(infoLegR);

        infoLegL.targetTrans.position -= new Vector3(0, 0, 0.4f);

        for (int i = 0; i < legList.Count; i++)
        {
            legList[i].currentTargetPos = legList[i].lastTargetPos = legList[i].targetTrans.position;
        }
    }


}
