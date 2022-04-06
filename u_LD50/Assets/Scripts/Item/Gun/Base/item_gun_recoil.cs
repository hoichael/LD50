using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_gun_recoil : MonoBehaviour
{
    [Header("Position Values")]

    [SerializeField]
    private float posOffset;

    [SerializeField]
    private float posFactorGrow;

    [SerializeField]
    private float posFactorReset;

    [SerializeField]
    private AnimationCurve posAnimCurve;

    private float posCurrentTarget;
    private float posCurrentOffset;
    private float posCurrentFactor;

/* -------------------------------------------------------------- */

    [Header("FOV Values")]

    [SerializeField]
    private float fovOffset;

    [SerializeField]
    private float fovFactorGrow;

    [SerializeField]
    private float fovFactorReset;

    private float fovCurrentTarget;
    public float fovCurrentOffset;
    private float fovCurrentFactor;

    public void Init()
    {
        posCurrentTarget = posOffset;
        posCurrentFactor = posFactorGrow;

        fovCurrentTarget = fovOffset;
        fovCurrentFactor = fovFactorGrow;
    }

    private void Update()
    {
        LerpPos();
        LerpFov();
    }

    private void LerpPos()
    {
        if (posCurrentOffset == posCurrentTarget && posCurrentTarget == 0) return;

        posCurrentOffset = Mathf.MoveTowards(posCurrentOffset, posCurrentTarget, posCurrentFactor * Time.deltaTime);

        transform.localPosition = Vector3.Lerp(
        Vector3.zero,
        new Vector3(0, 0, -posOffset),
        posAnimCurve.Evaluate(posCurrentOffset)
        );

        if (posCurrentTarget == posOffset && Mathf.Approximately(posCurrentOffset, posOffset))
        {
            posCurrentOffset = posOffset;
            posCurrentTarget = 0;
            posCurrentFactor = posFactorReset;
        }
    }

    private void LerpFov()
    {
        if (fovCurrentOffset == fovCurrentTarget && fovCurrentTarget == 0) return;

        fovCurrentOffset = Mathf.MoveTowards(fovCurrentOffset, fovCurrentTarget, fovCurrentFactor * Time.deltaTime);

        pl_state.Instance.currentFovOffsetRecoil = fovCurrentOffset;

        if (fovCurrentTarget == fovOffset && Mathf.Approximately(fovCurrentOffset, fovOffset))
        {
            fovCurrentOffset = fovOffset;
            fovCurrentTarget = 0;
            fovCurrentFactor = fovFactorReset;
        }
    }

}
