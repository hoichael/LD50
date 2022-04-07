using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_gun_trail : MonoBehaviour
{
    [SerializeField]
    private LineRenderer line;

    [SerializeField]
    private Color startColorBase;

    private Color startColorCurrent;

    [SerializeField]
    private Color endColorBase;

    private Color endColorCurrent;


    [SerializeField]
    private float fadeSpeed;

    private bool applyFade;

    public void Init(Vector3 origin, Vector3 hitPoint, float duration)
    {
        line.SetPosition(0, origin);
        line.SetPosition(1, hitPoint);
        line.startColor = startColorCurrent = startColorBase;
        line.endColor = endColorCurrent = endColorBase;
        line.enabled = true;

        applyFade = true;

        StartCoroutine(HandleDuration(duration));
    }

    private void Update()
    {
        if (!applyFade) return;
        startColorCurrent.a = Mathf.Lerp(startColorCurrent.a, 0, fadeSpeed * Time.deltaTime);
        endColorCurrent.a = Mathf.Lerp(endColorCurrent.a, 0, fadeSpeed * Time.deltaTime);

        line.startColor = startColorCurrent;
        line.endColor = endColorCurrent;
    }

    private IEnumerator HandleDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        applyFade = false;
        line.enabled = false;
    }
}
