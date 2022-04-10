using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_spider_0_legs : MonoBehaviour
{
    [Header("Leg Info Sets")]

    [SerializeField]
    private en_spider_leg_info infoLF;

    [SerializeField]
    private en_spider_leg_info infoLB;

    [SerializeField]
    private en_spider_leg_info infoRF;

    [SerializeField]
    private en_spider_leg_info infoRB;

    private List<en_spider_leg_info> infoList; // storing info in list for easy iteration

    [Header("Raycast Settings")]

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float raycastLength;


    private void Start()
    {
        infoList = new List<en_spider_leg_info>();
        infoList.Add(infoLF);
        infoList.Add(infoLB);
        infoList.Add(infoRF);
        infoList.Add(infoRB);
        print(infoList);
    }

    private void FixedUpdate()
    {
        UpdateTargetPositions();
    }

    private void UpdateTargetPositions()
    {
        for (int i = 0; i < infoList.Count; i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(infoList[i].raycastOrigin.position, infoList[i].raycastOrigin.up * -1, out hit, raycastLength, groundMask))
            {
                infoList[i].currentTargetPos = hit.point;

                // DB DB DB DB DB DB DB DB DB DB DB DB DB DB
                infoList[i].targetObj.transform.position = hit.point;
            }
        }
    }

}




[System.Serializable]
public class en_spider_leg_info
{
    public GameObject targetObj;
    public Transform raycastOrigin;
    public Vector3 currentTargetPos;
}