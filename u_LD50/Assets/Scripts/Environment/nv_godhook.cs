using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_godhook : MonoBehaviour, I_TakeItem
{
    [Header("Refs")]

    [SerializeField]
    private int_base intInfoRef;

    [SerializeField]
    private Transform itemHolder;

    [Header("Stock Settings")]

    [SerializeField]
    private List<GameObject> valueOneItems;

    [SerializeField]
    private List<GameObject> valueTwoItems;

    [SerializeField]
    private List<GameObject> valueThreeItems;

    [SerializeField]
    private List<GameObject> valueFourItems;

    [SerializeField]
    private List<GameObject> valueFiveItems;

    private Dictionary<int, List<GameObject>> itemDict = new Dictionary<int, List<GameObject>>();


    private item_base currentItemInfo;


    private void Start()
    {
        // populate itemDict;
        itemDict.Add(1, valueOneItems);
        itemDict.Add(2, valueTwoItems);
        itemDict.Add(3, valueThreeItems);
        itemDict.Add(4, valueFourItems);
        itemDict.Add(5, valueFiveItems);
    }

    public void Init(item_base itemInfo)
    {
     //   print("hook init with " + itemInfo.ID);

        // remove takesItem flag so Init won't be triggered while it already has item on it. dumb but the whole item system architecture is dumb
        intInfoRef.takesItem = false; 

        currentItemInfo = itemInfo;
        MoveItemToHook();
        StartCoroutine(DevDelay(itemInfo));
    }

    private void SpawnItem(GameObject itemToSpawn)
    {
        Destroy(currentItemInfo.gameObject);
        Instantiate(itemToSpawn, itemHolder.position, Quaternion.identity).GetComponent<Rigidbody>().isKinematic = true;
        intInfoRef.takesItem = true;
    }


    private void MoveItemToHook()
    {
        currentItemInfo.transform.SetParent(itemHolder);
        currentItemInfo.transform.localPosition = Vector3.zero;
        currentItemInfo.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private GameObject GetRandomItem(List<GameObject> list, string idToIgnore)
    {
        bool loop = true;
        int idx = 0;

        int safetyInt = 0;

        while(loop)
        {
            int randomIDX = Random.Range(0, list.Count);

            safetyInt++;
            if (safetyInt > 100)
                loop = false;

            if(list[randomIDX].GetComponent<item_base>().ID != idToIgnore)
            {
                loop = false;
                idx = randomIDX;
            }
        }
        
        return list[idx];
    }

    private IEnumerator DevDelay(item_base itemInfo)
    {
        yield return new WaitForSeconds(1.5f);

        SpawnItem(GetRandomItem(itemDict[itemInfo.value], itemInfo.ID));
    }

    // note: should make a variation of shotgun prefab with 0 shells attached for godhook to prevent "full shotgun farming".
    // not that important though
}
