using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_health_goatman : en_health_base
{
    [SerializeField]
    private en_ragdoll ragdoll;

    [SerializeField]
    private en_brain_base brain;

    [SerializeField]
    private FMODUnity.StudioEventEmitter sfxScreamEmitter;

    [SerializeField]
    private FMODUnity.StudioEventEmitter sfxDamageEmitter;

    public override void HandleDamage(dmg_base dmgInfo)
    {
        base.HandleDamage(dmgInfo);
        sfxScreamEmitter.Play();
    }

    protected override void HandleDeath(dmg_base dmgInfo)
    {
        base.HandleDeath(dmgInfo);

        sfxDamageEmitter.Play();

        transform.tag = "Corpse";

        brain.currentState.enabled = false;
        brain.enabled = false;

        ragdoll.ToggleRagdoll(true);

        ragdoll.rbRagRoot.AddForce(Vector3.up * 60f, ForceMode.Impulse);

        ragdoll.rbRagRoot.AddForce((transform.position - pl_state.Instance.GLOBAL_CAM_REF.transform.position).normalized * 95f, ForceMode.Impulse);

        StartCoroutine(StopSFX());
    }

    // not sure if this is necessary but never stopping the sound instances might clog up memory
    private IEnumerator StopSFX()
    {
        yield return new WaitForSeconds(3);
        sfxDamageEmitter.Stop();
        sfxScreamEmitter.Stop();
    }
}
