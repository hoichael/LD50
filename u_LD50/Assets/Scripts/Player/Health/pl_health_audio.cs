using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health_audio : MonoBehaviour
{
    [SerializeField]
    private FMODUnity.StudioEventEmitter fAmbienceEmitter;



    public void HealthChange()
    {
        UpdateAmbienceParam();
    }

    private void UpdateAmbienceParam()
    {
        float healthPercentage = (float)pl_state.Instance.health / (float)pl_settings.Instance.maxHealth;
        fAmbienceEmitter.SetParameter("Ambience Intensity", 1 - healthPercentage);
    }
}
