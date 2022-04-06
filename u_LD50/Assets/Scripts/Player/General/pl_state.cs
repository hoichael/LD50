using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_state : ut_singleton<pl_state>
{
    public bool grounded;

    public float camTilt;

    public float currentFovOffsetSprint;
    public float currentFovOffsetRecoil;

    public int health = 100;
    public float currentHungerMult = 1;
}
