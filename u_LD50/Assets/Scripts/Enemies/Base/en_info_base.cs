using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_info_base : MonoBehaviour
{
    [Header("Local Refs")]

    public Transform trans;
    public Rigidbody rb;
    public Animator anim;

    [Header("Settings")]

    public float knockbackMult; // basically "weight" property, but higher value = lower weight

    [Header("State")]

    public bool grounded;
}
