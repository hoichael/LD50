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

    private IEnumerator currentFlickerExec;

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

        if(!currentlyEnabled)
        {
            StopAllCoroutines();
        }
        else
        {
            StartCoroutine(FlickerCheck(10));
        }
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

    private IEnumerator FlickerCheck(float delay)
    {

        yield return new WaitForSeconds(delay);

        if(Random.Range(0, 10) > 5 && currentFlickerExec == null)
        {
            CallFlickerExec();
        }

        CallFlickerCheck();
    }

    private IEnumerator FlickerExec()
    {
        // mb play sfx for each flicker?

        lightPrimary.enabled = lightSecondary.enabled = false;

        yield return new WaitForSeconds(Random.Range(1f, 15) / 100);

        lightPrimary.enabled = lightSecondary.enabled = true;

        yield return new WaitForSeconds(Random.Range(1f, 30) / 100);

        lightPrimary.enabled = lightSecondary.enabled = false;

        yield return new WaitForSeconds(Random.Range(1f, 15) / 100);

        lightPrimary.enabled = lightSecondary.enabled = true;

        currentFlickerExec = null;
        if (Random.Range(0, 10) > 6)
        {
            CallFlickerExec();
        }
    }
    private void CallFlickerCheck()
    {
        StartCoroutine(FlickerCheck(Random.Range(4, 20)));
    }

    private void CallFlickerExec()
    {
        currentFlickerExec = FlickerExec();
        StartCoroutine(currentFlickerExec);
    }
}
