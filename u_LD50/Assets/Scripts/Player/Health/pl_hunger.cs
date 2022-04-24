using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_hunger : MonoBehaviour
{
    [SerializeField]
    private float interval;

    [SerializeField]
    private pl_health_damage healthDMG;

    [SerializeField]
    private dmg_base dmgInfo;

    private void Start()
    {
        StartCoroutine(HungerInterval());
    }

    private IEnumerator HungerInterval()
    {
        yield return new WaitForSeconds(interval);

        dmgInfo.dmgAmount = 1 * pl_state.Instance.currentHungerMult;
        healthDMG.HandleDamage(dmgInfo);

        RestartHungerRoutine();
    }

    private void RestartHungerRoutine()
    {
        StartCoroutine(HungerInterval());
    }
}
