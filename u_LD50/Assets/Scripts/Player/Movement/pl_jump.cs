using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_jump : MonoBehaviour
{
    [SerializeField]
    private pl_input input;

    [SerializeField]
    private Rigidbody rb;

    private void Update()
    {
        if(input.jumpKeyDown && pl_state.Instance.grounded)
        {
            InitJump();
        }
    }

    private void InitJump()
    {
        rb.AddForce(new Vector3(0, pl_settings.Instance.jumpForce, 0), ForceMode.Impulse);
    }
}
