using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject sunObj;

    [SerializeField]
    private float sensMin, sensMax, sensStep;

    [SerializeField]
    private Rigidbody plRB;

    [SerializeField]
    private pl_input plInput;

    [SerializeField]
    private pl_health_manager plHealthManager;

    [Header("UI Overlay Fade")]

    [SerializeField]
    private UnityEngine.UI.Image fadeImage;

    [SerializeField]
    private float fadeSpeed;

    [SerializeField]
    private AnimationCurve animCurve;

    private float fadeAnimProgress;

    private int currentFadeColor; // set to 0 or 255 depending on what kind of fade should be applied (death/spawn or rebirth)

    private float fadeOpacityStart, fadeOpacityTarget;

    private void Start()
    {
        sunObj.SetActive(false); // disable global directional light on start so I don't have to always disable it manually (i have it enabled in inspector for general visibility)

        currentFadeColor = 0;
        fadeOpacityStart = 255;
        fadeOpacityTarget = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            AdjustSens(1);
        }

        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            AdjustSens(-1);
        }

        if (fadeAnimProgress != 1) HandleFade();
    }

    private void AdjustSens(int mult) // mult = 1 or -1
    {
        pl_settings.Instance.mouseSens = Mathf.Clamp(pl_settings.Instance.mouseSens + sensStep * mult, sensMin, sensMax);
    }

    private void HandleFade()
    {
        fadeAnimProgress = Mathf.MoveTowards(fadeAnimProgress, 1, fadeSpeed * Time.deltaTime);

        float opacity = Mathf.Lerp(
            fadeOpacityStart,
            fadeOpacityTarget,
            animCurve.Evaluate(fadeAnimProgress)
            );

        fadeImage.color = new Color32((byte)currentFadeColor, (byte)currentFadeColor, (byte)currentFadeColor, (byte)Mathf.RoundToInt(opacity));
    }

    public void Death(Vector3 rebirthPos)
    {
        pl_state.Instance.currentlyDead = true;

        plRB.isKinematic = true;
        plInput.mouseX = plInput.mouseY = 0;
        plInput.enabled = false;

        fadeAnimProgress = 0;
        fadeOpacityStart = 0;
        fadeOpacityTarget = 255;

        if(rebirthPos == Vector3.zero)
        {
            currentFadeColor = 0;
            StartCoroutine(DelayedReload());
        }
        else
        {
            currentFadeColor = 255;
            StartCoroutine(DelayedRebirth(rebirthPos));
        }

    }

    private IEnumerator DelayedRebirth(Vector3 rebirthPos)
    {
        yield return new WaitForSeconds(5.5f);

        pl_state.Instance.GLOBAL_PL_TRANS_REF.position = rebirthPos;

        pl_state.Instance.currentlyDead = false;

        plRB.isKinematic = false;
        plInput.enabled = true;

        pl_state.Instance.health = pl_settings.Instance.maxHealth;

        plHealthManager.HandleHealthChange();

        fadeAnimProgress = 0;
        fadeOpacityStart = 255;
        fadeOpacityTarget = 0;
        currentFadeColor = 255;
    }

    private IEnumerator DelayedReload()
    {
        yield return new WaitForSeconds(5.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
