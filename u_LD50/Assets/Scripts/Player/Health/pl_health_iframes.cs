using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health_iframes : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image flashImage;

    [SerializeField]
    private CanvasGroup flashGroup;

    [SerializeField]
    private float flashSpeed;

    private float currentFlashProgress;

    [SerializeField]
    private AnimationCurve animCurve;

    void Awake()
    {
        flashGroup.alpha = 0;
        this.enabled = false;
    }

    private void OnEnable()
    {
        currentFlashProgress = 0;
    }

    private void Update()
    {
        if(currentFlashProgress == 1)
        {
            this.enabled = false;
        }

        currentFlashProgress = Mathf.MoveTowards(currentFlashProgress, 1, flashSpeed * Time.deltaTime);

        float opacity = Mathf.Lerp(
            0,
            0.18f,
            animCurve.Evaluate(Mathf.PingPong(currentFlashProgress, 0.5f))
            );

            flashGroup.alpha = opacity;
    }
}
