using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_gun_flash : MonoBehaviour
{
    [SerializeField]
    private Light lightComponent;

    [SerializeField]
    private float flashDuration;

    public void Init()
    {
        lightComponent.enabled = true;
        StartCoroutine(TurnOffLight());
    }

    private IEnumerator TurnOffLight()
    {
        yield return new WaitForSeconds(flashDuration);
        lightComponent.enabled = false;
    }
}
