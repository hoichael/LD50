using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_attack : en_state_base
{
    [SerializeField]
    private CapsuleCollider col;

    [SerializeField]
    private dmg_base dmgInfo;

    [SerializeField]
    private float dmgInterval;

    private float dmgTimer;

    private pl_health_damage plDamage;

    [SerializeField]
    private Transform modelTrans;

    [SerializeField]
    private Vector3 colOffset;

    [SerializeField]
    private Vector3 colDefaultPos;

    private void Start()
    {
        plDamage = pl_state.Instance.GLOBAL_PL_TRANS_REF.GetComponentInChildren<pl_health_damage>();
        colDefaultPos = col.center;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        info.rb.isKinematic = true;
        //   col.isTrigger = true;

        col.center = colOffset;

        info.trans.SetParent(pl_state.Instance.GLOBAL_CAM_REF.transform);
        /*
        info.trans.localPosition = new Vector3(0, -0.4f, 0) + pl_state.Instance.GLOBAL_CAM_REF.transform.forward * -0.15f;
        info.trans.localRotation = Quaternion.Euler(new Vector3(82, 0, 0))
        */

        info.trans.localPosition = new Vector3(0, -0.1f, 0) + pl_state.Instance.GLOBAL_CAM_REF.transform.forward * -0.25f;
        info.trans.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));

        modelTrans.localRotation = Quaternion.Euler(Vector3.zero);

        info.anim.SetBool("attack", true);
    }

    private void Update()
    {
        dmgTimer += Time.deltaTime;
        if(dmgTimer > dmgInterval)
        {
            dmgTimer = 0;
            DealDamage();
        }
    }

    private void DealDamage()
    {
        plDamage.HandleDamage(dmgInfo);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
            info.trans.SetParent(null);
     //   info.trans.parent = null;
        info.rb.isKinematic = false;
        col.center = colDefaultPos;
        //   col.isTrigger = false;
        modelTrans.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));

        info.anim.SetBool("attack", true);
    }
}
