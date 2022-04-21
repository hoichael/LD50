using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.PostProcessing;

public class pl_health_pp : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume ppVolume;

    [SerializeField]
    private PostProcessProfile ppProfile;

    private Vignette ppVignette;
    private Grain ppGrain;
    private ChromaticAberration ppChroma;
    private AutoExposure ppAutoEx;

    [SerializeField]
    private float grainMax;

    [SerializeField]
    private float chromaMax;

    [SerializeField]
    private float vignetteMax;


    [SerializeField]
    private float ppTriggerHpDivider;

    private float currentHealthPercentage;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private pl_input input;

    [SerializeField]
    private float deathAnimSpeed;

    private bool dead;

    private float currentDeathAnimProgress;

    private void Start()
    {
        ppProfile.TryGetSettings(out ppVignette);
        ppProfile.TryGetSettings(out ppGrain);
        ppProfile.TryGetSettings(out ppChroma);
        ppProfile.TryGetSettings(out ppAutoEx);

        ResetPostProcessing();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //    ppVignette.enabled.value = !ppVignette.enabled.value;
        //    ppVignette.intensity.value += 0.2f;
        //    ppVignette.color.value = new Color(0, 255, 0);
        }

        if(dead)
        {
            UpdatePostProcessing();
        }
    }

    public void HealthChange()
    {
        if (dead) return;

        if (pl_state.Instance.health < pl_settings.Instance.maxHealth / ppTriggerHpDivider)
        {
            currentHealthPercentage = pl_state.Instance.health / (pl_settings.Instance.maxHealth / ppTriggerHpDivider);
            UpdatePostProcessing();

            if (pl_state.Instance.health <= 0)
            {
                // this should definitely not be happening here, but thats ok.
                rb.isKinematic = true;
                input.enabled = false;
                ppAutoEx.active = true;
                dead = true;
            }
        }
        else
        {
            ResetPostProcessing();
        }
    }

    private void UpdatePostProcessing()
    {
        // MAP VIGNETTE VALUES TO HEALTH PERCENTAGE
        ppVignette.intensity.value = vignetteMax * (1 - currentHealthPercentage);
        if (ppVignette.intensity.value > vignetteMax) ppVignette.intensity.value = vignetteMax;

        //   print(currentHealthPercentage);
        //   print(vignetteMax / currentHealthPercentage);


        // MAP GRAIN VALUES TO HEALTH PERCENTAGE
        ppGrain.intensity.value = grainMax * (1 - currentHealthPercentage);
        if (ppGrain.intensity.value > grainMax) ppGrain.intensity.value = grainMax;


        // MAP CHROMA VALUES TO HEALTH PERCENTAGE
        ppChroma.intensity.value = chromaMax * (1 - currentHealthPercentage);
        if (ppChroma.intensity.value > grainMax) ppChroma.intensity.value = chromaMax;


        if (dead)
        {
            currentDeathAnimProgress = Mathf.MoveTowards(currentDeathAnimProgress, 1, deathAnimSpeed * Time.deltaTime);

            currentHealthPercentage = currentDeathAnimProgress;

            float luminenceLerp = Mathf.Lerp(
                0,
                -9,
                currentDeathAnimProgress
                );

            ppAutoEx.minLuminance.value = ppAutoEx.maxLuminance.value = luminenceLerp;
        }

    }

    private void ResetPostProcessing()
    {
        ppVignette.intensity.value = ppGrain.intensity.value = ppChroma.intensity.value = 0;
        ppAutoEx.minLuminance.value = ppAutoEx.maxLuminance.value = 0;
        ppAutoEx.active = false;
    }

    private void OnDisable()
    {
        ResetPostProcessing();
    }
}
