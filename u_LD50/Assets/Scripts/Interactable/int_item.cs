using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class int_item : int_base
{
    //   public bool isDynamic;

    public string itemType;

    public GameObject dynamicItemPrefab;

    public override void Init()
    {
        base.Init();
        Destroy(gameObject);
    }
}
