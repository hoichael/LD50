using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_swiper_health : en_health_base
{
    [SerializeField]
    private en_ragdoll ragdoll;

    [SerializeField]
    private GameObject states;

    [SerializeField]
    private en_brain_base brain;

    [SerializeField]
    private FMODUnity.StudioEventEmitter sfxDamage;

    public override void HandleDamage(dmg_base dmgInfo)
    {
        base.HandleDamage(dmgInfo);
        sfxDamage.Play();
    }

    protected override void HandleDeath(dmg_base dmgInfo)
    {
        base.HandleDeath(dmgInfo);
        transform.tag = "Corpse";

        //   brain.currentState.enabled = false;
        //   brain.enabled = false;
        states.SetActive(false);

        ragdoll.ToggleRagdoll(true);

        ragdoll.rbRagRoot.AddForce(Vector3.up * 60f, ForceMode.Impulse);

        ragdoll.rbRagRoot.AddForce((transform.position - pl_state.Instance.GLOBAL_CAM_REF.transform.position).normalized * 95f, ForceMode.Impulse);

        StartCoroutine(StopSFX());
    }

    private IEnumerator StopSFX()
    {
        yield return new WaitForSeconds(3f);

        sfxDamage.Stop();
    }
}
