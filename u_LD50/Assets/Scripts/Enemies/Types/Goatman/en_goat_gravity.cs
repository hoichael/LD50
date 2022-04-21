using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_goat_gravity : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Rigidbody rbRagRoot;

    private float baseGravity = -6.4f;
    private float growingForce = 1.05f;
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

            rbRagRoot.AddForce(new Vector3(0, baseGravity, 0), ForceMode.Acceleration);

            if (rb.velocity.y < 0)
            {
                growingForce += pl_settings.Instance.growthFactor;
                extraGravity = Mathf.Clamp(extraGravity * growingForce, 1f, 13f);
                rb.AddForce(new Vector3(0, -extraGravity, 0), ForceMode.Acceleration);

                rbRagRoot.AddForce(new Vector3(0, -extraGravity, 0), ForceMode.Acceleration);
            }
        }
        else
        {
            growingForce = 1.1f;
            extraGravity = 1f;
        }
    }
}
