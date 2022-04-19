using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_crate_debris : MonoBehaviour
{
    [SerializeField]
    private float physicsLifetime;

    [SerializeField]
    private Collider col;

    [SerializeField]
    private Rigidbody rb;

    private void OnEnable()
    {
        StartCoroutine(HandlePhysicsLifetime());
    }

    private IEnumerator HandlePhysicsLifetime()
    {
        yield return new WaitForSeconds(physicsLifetime);
        //   col.enabled = false;
        Destroy(col);
        Destroy(rb);
    }
}
