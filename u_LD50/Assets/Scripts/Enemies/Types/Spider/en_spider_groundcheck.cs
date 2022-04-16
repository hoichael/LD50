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

    private void Update()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        print("a");
        RaycastHit hit;
        if (Physics.Raycast(info.bodyTrans.position + Vector3.up * 1.2f, Vector3.up * -1, out hit, raycastBodyLength, groundMask))
        {
            print("b");
            info.rb.useGravity = false;
            info.grounded = true;
            //    transform.position = new Vector3(transform.position.x, hit.point.y + groundDistance, transform.position.z);
        }
        else
        {
            info.rb.useGravity = true;
            info.grounded = false;
        }
    }
}
