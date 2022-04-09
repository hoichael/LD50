using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_wep_base : item_base
{
    [SerializeField]
    private Collider damageCol;

 //   [SerializeField]
 //   private GameObject throwLogic;

    public override void Use()
    {
        base.Use();
        print("USE WEAPON");
    }


    private void OnDisable()
    {
     //   throwLogic.SetActive(true);
    }
}
