using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_hunger : MonoBehaviour
{
    [SerializeField]
    private float interval;

    [SerializeField]
    private pl_health_ui healthUI;

    private void Start()
    {
        StartCoroutine(HungerInterval());
    }

    private IEnumerator HungerInterval()
    {
        yield return new WaitForSeconds(interval);

        pl_state.Instance.health = pl_state.Instance.health - 1 * pl_state.Instance.currentHungerMult;
        healthUI.HealthChange();

        RestartHungerRoutine();
    }

    private void RestartHungerRoutine()
    {
        StartCoroutine(HungerInterval());
    }
}
