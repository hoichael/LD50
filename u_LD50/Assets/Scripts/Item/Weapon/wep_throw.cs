using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wep_throw : MonoBehaviour
{
    [SerializeField]
    private dmg_base dmgInfo;

    [SerializeField]
    private item_base itemInfo;

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
        print("throw enter trigger");
        if (col.transform.CompareTag("Enemy"))
        {
            print("throw col == enemy");
            dmgInfo.dmgAmount = Mathf.RoundToInt((dmgInfo.dmgAmount * itemInfo.rb.velocity.magnitude) * 0.4f);
            print(dmgInfo.dmgAmount);
            col.transform.GetComponent<en_health_base>().HandleDamage(dmgInfo);
         //   gameObject.SetActive(false);
        }
    }
}
