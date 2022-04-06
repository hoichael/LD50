using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_hunger : MonoBehaviour
{
    private float interval;

    private void Start()
    {
        interval = pl_settings.Instance.hungerInterval;
        StartCoroutine(HungerInterval());
    }

    private IEnumerator HungerInterval()
    {
        yield return new WaitForSeconds(interval);

        pl_state.Instance.health--;
        StartCoroutine(HungerInterval());
    }
}
