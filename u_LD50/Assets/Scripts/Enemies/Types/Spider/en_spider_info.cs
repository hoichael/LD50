using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_info : en_info_base
{
    public Transform bodyTrans;

    [Header("Leg Info Sets")]

    public en_spider_leg_info legLF;

    public en_spider_leg_info legLB;

    public en_spider_leg_info legRF;

    public en_spider_leg_info legRB;

    public List<en_spider_leg_info> legList; // storing info in list for easy iteration

    private void Awake()
    {
        legLF.oppositeLeg = legRF;
        legLB.oppositeLeg = legRB;
        legRF.oppositeLeg = legLF;
        legRB.oppositeLeg = legLB;

        legList = new List<en_spider_leg_info>();

        legList.Add(legLF);
        legList.Add(legLB);
        legList.Add(legRF);
        legList.Add(legRB);
    }
}

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
