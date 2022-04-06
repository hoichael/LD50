using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_D_flashlight : MonoBehaviour
{
    [SerializeField]
    private Light lightPrimary;

    [SerializeField]
    private Light lightSecondary;

    [SerializeField]
    private float xRotHide;

    private float xRotCurrent;

    [SerializeField]
    private float hideAnimLerpFactor;

    private float currentLerpTarget;

    private bool currentlyEnabled;

    private void Start()
    {
        currentlyEnabled = lightPrimary.enabled;

        if(!currentlyEnabled)
        {
            xRotCurrent = currentLerpTarget = xRotHide;
            transform.localRotation = Quaternion.Euler(xRotCurrent, 0, 0);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if (transform.localRotation.x == currentLerpTarget) return;

        HandleToggleAnim();
    }

    private void ToggleFlashlight()
    {
        currentLerpTarget = currentlyEnabled ? xRotHide : 0;

        currentlyEnabled = !currentlyEnabled;

        lightPrimary.enabled = lightSecondary.enabled = currentlyEnabled;
    }

    private void HandleToggleAnim()
    {
        xRotCurrent = Mathf.Lerp(
            xRotCurrent,
            currentLerpTarget,
            hideAnimLerpFactor * Time.deltaTime
            );

        transform.localRotation = Quaternion.Euler(xRotCurrent, 0, 0);
    }
}
