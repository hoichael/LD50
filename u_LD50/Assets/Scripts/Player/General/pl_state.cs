using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_state : ut_singleton<pl_state>
{
    public bool grounded;

    public float camTilt;

    public float currentFovOffsetSprint;
    public float currentFovOffsetRecoil;

    public int health = 1000;
    public int currentHungerMult = 1;
}
