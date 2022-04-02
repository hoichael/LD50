using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_D_flashlight : MonoBehaviour
{
    [SerializeField]
    private Light flashlight;

    [SerializeField]
    private float xRotHide;

    private float xRotCurrent;

    [SerializeField]
    private float hideAnimLerpFactor;

    private float currentLerpTarget;

    private bool currentlyEnabled;

    private void Start()
    {
        currentlyEnabled = flashlight.enabled;

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
        flashlight.enabled = currentlyEnabled;
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
