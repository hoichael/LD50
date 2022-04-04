using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class item_base : MonoBehaviour
{
    public Rigidbody rb;

    public Collider col;

    // public GameObject modelObj;

    public string type;

    int size; // used later for bag

    int value; // used later for shop

    public virtual void Use()
    {
        print("init Use of type " + type);
    }
}
