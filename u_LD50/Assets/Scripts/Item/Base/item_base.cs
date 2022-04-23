using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class item_base : MonoBehaviour
{
    public string ID;

    public Rigidbody rb;

    public GameObject col; // change type from Collider to GameObject to support compound colliders (set obj inactive instead of toggling col component)

    public string type;

    public int value; // used later for shop

    public virtual void Use()
    {
    //    print("init Use of type " + type);
    }
}
