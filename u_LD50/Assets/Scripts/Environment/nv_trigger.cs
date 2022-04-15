using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_trigger : MonoBehaviour
{
    // obj this script is attached to needs to be on layer that only collides with player

    [SerializeField]
    private List<GameObject> objToTriggerList = new List<GameObject>();

    [SerializeField]
    private bool setObjectsActive;

    [SerializeField]
    private bool destroyOnTrigger;



    private void OnTriggerEnter(Collider other)
    {
        print("PLAYER ENTER");

        for(int i = 0; i < objToTriggerList.Count; i++)
        {
            objToTriggerList[i].SetActive(setObjectsActive);
        }

        if (destroyOnTrigger) Destroy(gameObject);
    }
}
