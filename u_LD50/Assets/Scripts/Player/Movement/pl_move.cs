using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_move : MonoBehaviour
{
    [SerializeField]
    private pl_input input;

    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private Rigidbody rb;

    private Vector3 currentDir;

    private void Awake()
    {
        rb.freezeRotation = true;
    }

    private void Update()
    {
        GetMoveDirection();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void GetMoveDirection()
    {
        currentDir = orientation.forward * input.moveY + orientation.right * input.moveX;
        currentDir = currentDir.normalized;
    }

    private void ApplyMovement()
    {
        rb.AddForce(currentDir * pl_settings.Instance.moveSpeed, ForceMode.Acceleration);
    }
}
