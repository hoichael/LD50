using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class int_base : MonoBehaviour
{
    protected string ID;
    public virtual void Init()
    {
        print("pickup " + ID);
        Destroy(gameObject);
    }
}
