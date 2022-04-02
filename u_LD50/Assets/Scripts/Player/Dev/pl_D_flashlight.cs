using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_D_flashlight : MonoBehaviour
{
    [SerializeField]
    private Light flashlight;

    private bool currentlyEnabled;

    private void Start()
    {
        currentlyEnabled = flashlight.enabled;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    private void ToggleFlashlight()
    {
        currentlyEnabled = !currentlyEnabled;
        flashlight.enabled = currentlyEnabled;
    }
}
