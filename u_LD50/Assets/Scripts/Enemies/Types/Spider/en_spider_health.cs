using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_health : en_health_base
{
    [SerializeField]
    private en_brain_base brain;

    [SerializeField]
    private en_info_base info;

    [SerializeField]
    private GameObject states;

    public override void HandleDamage(dmg_base dmgInfo)
    {
        base.HandleDamage(dmgInfo);

        HandleKnockback();
        brain.ChangeState("knock");

    }

    private void HandleKnockback()
    {
        info.rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        info.rb.AddForce((info.trans.position - pl_state.Instance.GLOBAL_CAM_REF.transform.position).normalized * 3f, ForceMode.Impulse);
    }

    protected override void HandleDeath(dmg_base dmgInfo)
    {
        base.HandleDeath(dmgInfo);

        /*
        brain.currentState.enabled = false;
        brain.enabled = false;
        */
        states.SetActive(false);

        info.trans.SetParent(null);
        info.trans.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.5f, 0);


        info.rb.freezeRotation = false;
        info.rb.AddTorque(new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1)) * Random.Range(30, 40), ForceMode.Impulse);

        info.trans.tag = "Corpse";
        info.anim.SetBool("dead", true);

        DeathRoutine();
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(9f);
        info.rb.isKinematic = true;
        info.col.enabled = false;
    }


    /*
    protected override void HandleDeath(dmg_base dmgInfo)
    {
        base.HandleDeath(dmgInfo);
    }
    */
}
