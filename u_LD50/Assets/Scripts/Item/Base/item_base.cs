using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class item_base : MonoBehaviour
{
    public string ID;

    public Rigidbody rb;

    public Collider col;

    public string type;

    public int value; // used later for shop

    public virtual void Use()
    {
    //    print("init Use of type " + type);
    }
}
