using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_spider_groundcheck : MonoBehaviour
{

    [SerializeField]
    private en_spider_info info;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float raycastBodyLength;

    [SerializeField]
    private en_spider_legs_ground legsGround;

    [SerializeField]
    private en_spider_legs_air legsAir;

    private void Update()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(info.bodyTrans.position + Vector3.up * 1.2f, Vector3.up * -1, out hit, raycastBodyLength, groundMask))
        {
            info.rb.useGravity = false;
            info.grounded = true;

            legsAir.enabled = false;
            legsGround.enabled = true;
        }
        else
        {
            info.rb.useGravity = true;
            info.grounded = false;

            legsAir.enabled = true;
            legsGround.enabled = false;
        }
    }
}
