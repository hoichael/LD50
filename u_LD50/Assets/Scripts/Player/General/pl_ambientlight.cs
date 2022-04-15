using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_ambientlight : MonoBehaviour
{
    [SerializeField]
    private Light lightSource;

    [SerializeField]
    private float minLightRange;

    private float defaultLightRange;

    [SerializeField]
    private Transform monolithTrans;

    private Vector3 monolithPos;

    [SerializeField]
    private float initDistance;

    [SerializeField]
    private float maxDistance;

    private Transform thisTrans;

    private float currentDistance;

    private void Start()
    {    
        // caching these for performance bc why not
        monolithPos = monolithTrans.position;
        thisTrans = transform;

        defaultLightRange = lightSource.range;
    }

    private void Update()
    {
        currentDistance = Vector3.Distance(thisTrans.position, monolithPos);
       
        if(currentDistance <= initDistance)
        {
            lightSource.range = defaultLightRange;
        }
        else
        {
            currentDistance -= initDistance;

            if (currentDistance > maxDistance)
            {
                lightSource.range = minLightRange;
            }
            else
            {
                HandleDistance();
            }
        }       
    }

    private void HandleDistance()
    {
        lightSource.range = Mathf.Lerp(
            defaultLightRange,
            minLightRange,
            Mathf.Clamp01(currentDistance / maxDistance)
            );
    }
}
