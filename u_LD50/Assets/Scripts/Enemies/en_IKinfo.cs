using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class en_IKinfo
{
    public Transform targetTrans;

    public Transform raycastOrigin;
    public Vector3 lastTargetPos;
    public Vector3 currentTargetPos;
    public Vector3 currentRayPos;
    public Vector3 currentRayNormal;

    public en_IKinfo oppositeLeg;

    public bool grounded;
    public float currentAnimProgress;
}
