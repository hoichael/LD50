using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_godhook : MonoBehaviour
{
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

    private Dictionary<int, List<GameObject>> itemDict;

    private void Start()
    {
        return;
        // populate itemDict;
        itemDict.Add(1, valueOneItems);
        itemDict.Add(1, valueTwoItems);
        itemDict.Add(1, valueThreeItems);
        itemDict.Add(1, valueFourItems);
        itemDict.Add(1, valueFiveItems);

    }


    private GameObject GetRandomItem(List<GameObject> list, string idToIgnore)
    {
        bool loop = true;
        int idx = 0;

        while(loop)
        {
            int randomIDX = Random.Range(0, list.Count);

            if(list[randomIDX].GetComponent<item_base>().ID != idToIgnore)
            {
                loop = false;
                idx = randomIDX;
            }
        }
        
        return list[idx];
    }

    // note: should make a variation of shotgun prefab with 0 shells attached for godhook to prevent "full shotgun farming".
    // not that important though
}
