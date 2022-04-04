using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class int_base : MonoBehaviour
{
    [SerializeField]
    protected string ID;

    public bool isItem;

    public virtual void Init()
    {
    //    print("pickup " + ID);
    }
}
