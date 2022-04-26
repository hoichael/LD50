using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wep_throw : MonoBehaviour
{
    [SerializeField]
    private dmg_base dmgInfo;

    [SerializeField]
    private item_base itemInfo;

    [SerializeField]
    private FMODUnity.EventReference sfxImpact;

    /*
    private void OnTriggerEnter(Collider col)
    {
        print("throw enter trigger");
        if (col.CompareTag("Enemy"))
        {
            print("throw col == enemy");
            dmgInfo.dmgAmount = Mathf.RoundToInt(dmgInfo.dmgAmount * itemInfo.rb.velocity.magnitude);
            col.GetComponent<en_health_base>().HandleDamage(dmgInfo);
            gameObject.SetActive(false);
        }
    }
    */

    private void OnCollisionEnter(Collision col)
    {
        if (itemInfo.enabled) return;

        FMODUnity.RuntimeManager.PlayOneShot(sfxImpact, transform.position);

        if (col.transform.CompareTag("Enemy"))
        {
            dmgInfo.dmgAmount = Mathf.RoundToInt((dmgInfo.dmgAmount * itemInfo.rb.velocity.magnitude) * 0.4f);
            dmgInfo.force = 1 * (itemInfo.rb.velocity.magnitude * 0.1f);

            print(1 * (itemInfo.rb.velocity.magnitude * 0.1f));

            dmgInfo.hitPos = transform.position;
            col.transform.GetComponent<en_health_base>().HandleDamage(dmgInfo);
         //   gameObject.SetActive(false);
        }
    }
}
