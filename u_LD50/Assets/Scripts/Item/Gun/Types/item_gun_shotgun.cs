using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_gun_shotgun : item_gun_base
{
    [Header("Shotgun Settings")]

//    [SerializeField]
//    private int bulletAmount;

    [SerializeField]
    private float rayRange;

    [Range(0.05f, 1f)]
    [SerializeField]
    private float offsetRange;

    private float offsetRangeHalf;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private dmg_hitscan dmgInfo;

    [SerializeField]
    private Transform trailContainer;

    [SerializeField]
    private List<item_gun_trail> trailList;

    private float trailDuration;

    private void Start()
    {
        offsetRangeHalf = offsetRange * 0.5f;
        trailDuration = fireRate * 0.8f;
    }

    protected override void Shoot()
    {
        base.Shoot();

        StartCoroutine(HandleTrailContainer());
       
        for(int i = 0; i < trailList.Count; i++)
        {
            HandleRay(i);
        }
    }

    private void HandleRay(int trailIndex)
    {
        Vector3 bulletDirection = GetBulletDirection();

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, bulletDirection, out hit, rayRange, ~playerLayerMask))
        {
            if(hit.transform != null)
            {
                HandleTrail(hit.point, trailIndex);

                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    dmgInfo.origin = firePoint.position;
                    hit.transform.GetComponent<en_health_base>().HandleDamage(dmgInfo);
                }
            }
        }
        else
        {
            HandleTrail(firePoint.position + bulletDirection * rayRange, trailIndex);
        }
    }

    private void HandleTrail(Vector3 hitPoint, int listIndex)
    {
        trailList[listIndex].Init(firePoint.position, hitPoint, trailDuration);
    }

    private Vector3 GetBulletDirection()
    {
        Vector3 offset = new Vector3(
            Random.Range(-offsetRangeHalf, offsetRangeHalf),
            Random.Range(-offsetRangeHalf, offsetRangeHalf),
            Random.Range(-offsetRangeHalf, offsetRangeHalf)
            );
        
        return pl_state.Instance.GLOBAL_CAM_REF.transform.forward + offset;
    }

    // this is kinda ugly. but way more performant than 6 prefab instantiations + GetComponent calls for each shot.
    // external pool would probably be better though
    private IEnumerator HandleTrailContainer()
    {
        trailContainer.localScale = new Vector3(1, 1, 1);  // dont even ask
        trailContainer.SetParent(null);
        trailContainer.position = Vector3.zero;
        trailContainer.rotation = Quaternion.identity;

        trailContainer.localScale = new Vector3(1, 1, 1);  // dont even ask

        yield return new WaitForSeconds(trailDuration);
        trailContainer.localScale = new Vector3(1, 1, 1);  // dont even ask
        trailContainer.SetParent(this.transform);
        trailContainer.localPosition = Vector3.zero;
        trailContainer.localRotation = Quaternion.identity;
        trailContainer.localScale = new Vector3(1, 1, 1);  // dont even ask
    }
}
