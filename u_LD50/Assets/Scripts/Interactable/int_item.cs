using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class int_item : int_base
{
    //   public bool isDynamic;

    public string itemType;

    public override void Init()
    {
        base.Init();

        // remove "Interactable" tag that pl_interact raycast looks for
    //    tag.Remove(0); // I dont think this does what I thought it does

     //   Destroy(gameObject);
    }
}
