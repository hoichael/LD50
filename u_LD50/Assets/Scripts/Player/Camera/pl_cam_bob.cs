using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_cam_bob : MonoBehaviour
{
    [SerializeField]
    private pl_input input;

    [SerializeField]
    private float bobOffsetY;

    [SerializeField]
    private float lerpFactor;

    [SerializeField]
    private float currentLerpTarget;

    [SerializeField]
    private float currentOffsetY;

    private float defaultCamY;

    [SerializeField]
    private int currentDirMult = 1;

    private void Start()
    {
        defaultCamY = transform.localPosition.y;
    }

    private void Update()
    {
        if (input.moveX == 0 && input.moveY == 0)
        {
            currentLerpTarget = 0;
        }
        else
        {
            currentLerpTarget = bobOffsetY * currentDirMult;
        }

        ////////////////////////////////////////////
        
        transform.position = new Vector3(transform.position.x, 0.5f + currentOffsetY, transform.position.z);

        ////////////////////////////////////////////
        
        if (currentLerpTarget == 0)
        {
            if (Mathf.Abs(currentLerpTarget * 0.1f - currentOffsetY) < 0.001f)
            {
                currentOffsetY = 0;
            }
        }
        else if (Mathf.Abs(currentLerpTarget * 0.1f - currentOffsetY) < 0.001f)
        {
            currentDirMult *= -1;
        }

        currentOffsetY = Mathf.Lerp(
        currentOffsetY,
        currentLerpTarget,
        lerpFactor * Time.deltaTime);
    }
}
