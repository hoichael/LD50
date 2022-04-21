using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_legs_air : MonoBehaviour
{
    [SerializeField]
    private en_spider_info info;

    [SerializeField]
    private AnimationCurve animCurve;

    [SerializeField]
    private float legMoveSpeed;

    private float currentAnimProgress;

    private void OnEnable()
    {
        for(int i = 0; i < info.legList.Count; i++)
        {
            info.legList[i].currentAnimProgress = 0;
            info.legList[i].lastTargetPos = info.legList[i].currentTargetPos = info.legList[i].targetTrans.position;
        }

        currentAnimProgress = 0;
    }

    private void Update()
    {
        if(currentAnimProgress != 1)
        MoveLegs();
    }

    private void MoveLegs()
    {

        for (int i = 0; i < info.legList.Count; i++)
        {
            currentAnimProgress = Mathf.MoveTowards(currentAnimProgress, 1, legMoveSpeed * Time.deltaTime);

            info.legList[i].targetTrans.position = Vector3.Lerp(
                info.legList[i].lastTargetPos,
                info.legList[i].airTargetTrans.position,
                animCurve.Evaluate(currentAnimProgress)
                );
        }

    }
}
