using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health_ui : MonoBehaviour
{
    [SerializeField]
    private Transform healthBarTrans;

    [SerializeField]
    private pl_health_pp healthPP;

    [SerializeField]
    private FMODUnity.StudioEventEmitter fAmbienceEmitter;

    private float healthBarDefaultScaleX;
    private float healthBarDefaultScaleY;
    private float healthBarDefaultScaleZ;

    private void Start()
    {
        healthBarDefaultScaleX = healthBarTrans.localScale.x;
        healthBarDefaultScaleY = healthBarTrans.localScale.y;
        healthBarDefaultScaleZ = healthBarTrans.localScale.z;
    }

    public void HealthChange()
    {
        healthPP.HealthChange();

        UpdateAmbienceParam();

        healthBarTrans.localScale = new Vector3(pl_state.Instance.health * (healthBarDefaultScaleX / 1000), 
            healthBarDefaultScaleY, healthBarDefaultScaleZ);

        if(healthBarTrans.localScale.x < 0)
        {
            healthBarTrans.localScale = Vector3.zero;
        }
    }

    private void UpdateAmbienceParam()
    {
        float healthPercentage = (float)pl_state.Instance.health / (float)pl_settings.Instance.maxHealth;
        fAmbienceEmitter.SetParameter("Ambience Intensity", 1 - healthPercentage);
    }
}
