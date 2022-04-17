using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject sunObj;

    [SerializeField]
    private float sensMin, sensMax, sensStep;

    private void Start()
    {
        sunObj.SetActive(false); // disable global directional light on start so I don't have to always disable it manually (i have it enabled in inspector for general visibility)
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
    }

    private void AdjustSens(int mult) // mult = 1 or -1
    {
        pl_settings.Instance.mouseSens = Mathf.Clamp(pl_settings.Instance.mouseSens + sensStep * mult, sensMin, sensMax);
    }
}
