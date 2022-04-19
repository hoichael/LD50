using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_crate : MonoBehaviour
{
    [SerializeField]
    private GameObject crateModelAndCol;

    [SerializeField]
    List<GameObject> contentsList = new List<GameObject>();

    [SerializeField]
    List<GameObject> debrisList = new List<GameObject>();

    [SerializeField]
    private Transform debrisForcePos;

    [SerializeField]
    private int minAmountContent, maxAmountContent;

    [SerializeField]
    private float minForceDebris, maxForceDebris, minTorqueDebris, maxTorqueDebris;

    [SerializeField]
    private float minForceContents, maxForceContents, minTorqueContents, maxTorqueContents;

    private bool alreadyTriggered;

    public void Init()
    {
        if (alreadyTriggered) return;

        alreadyTriggered = true;

        Destroy(crateModelAndCol);

        HandleDebris();

        int itemsAmount = Random.Range(minAmountContent, maxAmountContent + 1);
        for(int i = 0; i <= itemsAmount; i++)
        {
            HandleItemSpawn();
        }
    }

    private void HandleDebris()
    {
        for(int i = 0; i < debrisList.Count; i++)
        {
            debrisList[i].SetActive(true);
            Rigidbody rb = debrisList[i].GetComponent<Rigidbody>();
            rb.AddExplosionForce(Random.Range(minForceDebris, maxForceDebris), debrisForcePos.position, 2f);
            rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * Random.Range(minTorqueDebris, maxTorqueDebris), ForceMode.Impulse);
        }
    }

    private void HandleItemSpawn()
    {
        GameObject item = Instantiate(contentsList[Random.Range(1, contentsList.Count)], debrisForcePos.position, Quaternion.identity);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * Random.Range(minForceContents, maxForceContents), ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * Random.Range(minTorqueContents, maxTorqueContents), ForceMode.Impulse);
    }
}
