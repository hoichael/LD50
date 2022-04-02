using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_gravity : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    private float baseGravity;
    private float growingForce = 1.1f;
    private float extraGravity = 1f;

    private void Start()
    {
        baseGravity = pl_settings.Instance.baseGravity;
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        // values need more tweaking
        if (rb.velocity.y != 0)
        {
            // apply base gravity
            rb.AddForce(new Vector3(rb.velocity.x, baseGravity, rb.velocity.z), ForceMode.Acceleration);

            // handle additional gravity
            if (rb.velocity.y < 0)
            {
                growingForce += pl_settings.Instance.growthFactor;
                extraGravity = Mathf.Clamp(extraGravity * growingForce, 1f, 16f);
                rb.AddForce(new Vector3(rb.velocity.x, -extraGravity, rb.velocity.z), ForceMode.Acceleration);
            }
        }
        else
        {
            growingForce = 1.1f;
            extraGravity = 1f;
        }

    }
}
