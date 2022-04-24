using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_wep_base : item_base
{
    [SerializeField]
    private dmg_base dmgInfo;

    public Transform itemAnchor;

    [SerializeField]
    private float animSpeedSwing;

    [SerializeField]
    private float animSpeedReset;

    [SerializeField]
    private Vector3 offsetPos;

    [SerializeField]
    private Vector3 offsetRot;

    private float currentLerpProgress;

    private bool currentlyAttacking;

    [SerializeField]
    private AnimationCurve animCurveSwing;

    [SerializeField]
    private AnimationCurve animCurveReset;

    private bool currentlySwinging;

    [SerializeField]
    private Vector3 hitboxHalfExtents;

    [SerializeField]
    private LayerMask enemiesLayerMask;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private FMODUnity.EventReference sfxImpact;

    public override void Use()
    {
        if (currentlyAttacking) return;

        base.Use();

        currentlyAttacking = true;
        currentlySwinging = true;
        currentLerpProgress = 0;
    }

    private void Update()
    {
        if (!currentlyAttacking) return;

        if(currentlySwinging)
        {
            HandleSwing();
        }
        else
        {
            HandleReset();
        }
    }

    private void HandleSwing()
    {
        currentLerpProgress = Mathf.MoveTowards(currentLerpProgress, 1, animSpeedSwing * Time.deltaTime);

        itemAnchor.transform.localPosition = Vector3.Lerp(
            Vector3.zero,
            offsetPos,
            animCurveSwing.Evaluate(Mathf.PingPong(currentLerpProgress, 0.5f))
            );

        itemAnchor.transform.localRotation = Quaternion.Lerp(
            Quaternion.Euler(Vector3.zero),
            Quaternion.Euler(offsetRot),
            animCurveSwing.Evaluate(currentLerpProgress)
            );

        if (currentLerpProgress == 1)
        {
            currentLerpProgress = 0;
            HandleHitbox();
            currentlySwinging = false;
        }
    }

    private void HandleReset()
    {
        currentLerpProgress = Mathf.MoveTowards(currentLerpProgress, 1, animSpeedReset * Time.deltaTime);

        itemAnchor.transform.localRotation = Quaternion.Lerp(
            Quaternion.Euler(offsetRot),
            Quaternion.Euler(Vector3.zero),
            animCurveReset.Evaluate(currentLerpProgress)
            );


        if (currentLerpProgress == 1)
        {
            currentlyAttacking = false;
        }
    }

    private void HandleHitbox()
    {
        // -------------------------------- HITBOX VISUALIZER --------------------------------

        //var db = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //db.transform.position = pl_state.Instance.GLOBAL_CAM_REF.transform.position + pl_state.Instance.GLOBAL_CAM_REF.transform.forward;
        //db.transform.localScale = hitboxHalfExtents * 2;
        //db.transform.rotation = Quaternion.identity;
        //db.GetComponent<Collider>().enabled = false;

        Collider[] hitColliders = Physics.OverlapBox(
            pl_state.Instance.GLOBAL_CAM_REF.transform.position + pl_state.Instance.GLOBAL_CAM_REF.transform.forward, 
            hitboxHalfExtents, 
            Quaternion.identity, 
            ~playerLayerMask
            );

        if(hitColliders.Length != 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot(sfxImpact, transform.position);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].CompareTag("Enemy"))
                {
                    hitColliders[i].GetComponentInParent<en_health_base>().HandleDamage(dmgInfo);
                }
            }
        }
    }
}
