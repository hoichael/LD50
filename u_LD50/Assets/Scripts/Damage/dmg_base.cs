using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class dmg_base
{
    [Header("Base Values")]

    public int dmgAmount;
    public string type; // melee / throw / hitscan / fall
    public float force; // used for knockback
    public Vector3 hitPos;
}
