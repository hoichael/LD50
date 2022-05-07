using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_jump : MonoBehaviour
{
    [SerializeField]
    private pl_input input;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float maxSlopeNormal;

    private void Update()
    {
        if(input.jumpKeyDown && pl_state.Instance.grounded)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f, groundMask))
            {
                if(Mathf.Abs(hit.normal.x) < maxSlopeNormal && Mathf.Abs(hit.normal.z) < maxSlopeNormal) 
                {
                    InitJump();
                }
            }
        }
    }

    private void InitJump()
    {
        rb.AddForce(new Vector3(0, pl_settings.Instance.jumpForce, 0), ForceMode.Impulse);
    }
}
