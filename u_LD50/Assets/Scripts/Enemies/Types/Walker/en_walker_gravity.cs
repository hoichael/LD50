using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_walker_gravity : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    private float baseGravity = -8f;
    private float growingForce = 1.1f;
    private float extraGravity = 1f;

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (rb.velocity.y != 0)
        {
            rb.AddForce(new Vector3(0, baseGravity, 0), ForceMode.Acceleration);

            if (rb.velocity.y < 0)
            {
                growingForce += pl_settings.Instance.growthFactor;
                extraGravity = Mathf.Clamp(extraGravity * growingForce, 1f, 16f);
                rb.AddForce(new Vector3(0, -extraGravity, 0), ForceMode.Acceleration);
            }
        }
        else
        {
            growingForce = 1.1f;
            extraGravity = 1f;
        }
    }
}
