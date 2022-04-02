using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_settings : ut_singleton<pl_settings>
{
    [Header("Camera")]
    public float mouseSens;
    public float camTiltMoveAmount;
    public float camTiltMoveFactor;

    [Header("Move")]
    public float moveSpeed;
    public float moveAirMult;
    public float dragGround;
    public float dragAir;

    [Header("Jump")]
    public float jumpForce;

    [Header("Gravity")]
    public float baseGravity;
    public float growthFactor;

    [Header("Misc")]
    public float groundCheckRadius;
}
