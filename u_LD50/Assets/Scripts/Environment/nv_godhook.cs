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

    [SerializeField]
    private Light lightSpot;

    [SerializeField]
    private Light lightPointA;

    [SerializeField]
    private Light lightPointB;

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

    [Header("Item Animation")]

    private Transform currentItemTrans;

    [SerializeField]
    private float itemAnimSpeed;

    [SerializeField]
    private AnimationCurve itemAnimCurve;

    private float animProgressItem;
    private Vector3 itemInitPos;
    private Quaternion itemInitRot;

    [Header("Rope Bob")]

    [SerializeField]
    private Vector3 ropeBobOffset;

    [SerializeField]
    private AnimationCurve ropeBobCurve;

    [SerializeField]
    private float ropeAnimBobSpeed;

    private float animProgressRopeBob;

    [Header("Rope Up")]

    [SerializeField]
    private Vector3 ropeOffset;

    [SerializeField]
    private float ropeAnimUpSpeed;

    [SerializeField]
    private AnimationCurve ropeAnimUpCurve;

    private float animProgressRopeUp;

    [Header("Rope Down")]

    [SerializeField]
    private float ropeAnimDownSpeed;

    [SerializeField]
    private AnimationCurve ropeAnimDownCurve;

    private float animProgressRopeDown;

    [Header("Stock Settings")]
    private float defaultSpotAngle;
    private float defaultSpotRange;
    private float defaultPointIntensity;




    private Vector3 defaultPos;
    private bool checkForItemTaken;

    private void Start()
    {
        animProgressItem = animProgressRopeBob = animProgressRopeUp = animProgressRopeDown = 1;
        defaultSpotAngle = lightSpot.spotAngle;
        defaultSpotRange = lightSpot.range;
        defaultPointIntensity = lightPointA.intensity;

        defaultPos = transform.position;

        // populate itemDict;
        itemDict.Add(1, valueOneItems);
        itemDict.Add(2, valueTwoItems);
        itemDict.Add(3, valueThreeItems);
        itemDict.Add(4, valueFourItems);
        itemDict.Add(5, valueFiveItems);
    }

    private void Update()
    {
        if(animProgressItem != 1)
        {
            MoveItemToHook();
        }
        else if(animProgressRopeBob != 1)
        {
            HandleRopeBob();
        }
        else if(animProgressRopeUp != 1)
        {
            HandleRopeUp();
        }
        else if(animProgressRopeDown != 1)
        {
            HandleRopeDown();
        }


        // this is hacky as fuck but probably the best way to get this done without refactoring large parts of the item system. no time for that
        if (!checkForItemTaken) return;
        if(currentItemInfo.col.enabled == false)
        {
            print("ITEM TAKEN");
            checkForItemTaken = false;
            intInfoRef.takesItem = true;
        }
    }

    public void Init(item_base itemInfo)
    {
     //   print("hook init with " + itemInfo.ID);

        // remove takesItem flag so Init won't be triggered while it already has item on it. dumb but the whole item system architecture is dumb
        intInfoRef.takesItem = false; 

        currentItemInfo = itemInfo;
        currentItemTrans = itemInfo.transform;
        currentItemTrans.SetParent(itemHolder);
        itemInitPos = currentItemTrans.localPosition;
        itemInitRot = currentItemTrans.localRotation;
        animProgressItem = 0;

    //    StartCoroutine(DevDelay(itemInfo));
    }


    private void MoveItemToHook()
    {
        /*
        currentItemTrans.SetParent(itemHolder);
        currentItemTrans.localPosition = Vector3.zero;
        currentItemTrans.localRotation = Quaternion.Euler(Vector3.zero);
        */

        animProgressItem = Mathf.MoveTowards(animProgressItem, 1, itemAnimSpeed * Time.deltaTime);

        currentItemTrans.localPosition = Vector3.Lerp(
            itemInitPos,
            Vector3.zero,
            itemAnimCurve.Evaluate(animProgressItem)
            );

        if(animProgressItem == 1)
        {
            animProgressRopeBob = 0;
        }
    }

    private void HandleRopeBob()
    {
        animProgressRopeBob = Mathf.MoveTowards(animProgressRopeBob, 1, ropeAnimBobSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(
            defaultPos,
            defaultPos + ropeBobOffset,
            ropeBobCurve.Evaluate(animProgressRopeBob)
            );

        if (animProgressRopeBob == 1)
        {
            animProgressRopeUp = 0;
        }
    }

    private void HandleRopeUp()
    {
        animProgressRopeUp = Mathf.MoveTowards(animProgressRopeUp, 1, ropeAnimUpSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(
            defaultPos + ropeBobOffset,
            defaultPos + ropeOffset,
            ropeAnimUpCurve.Evaluate(animProgressRopeUp)
            );

        lightSpot.spotAngle = lightSpot.innerSpotAngle = Mathf.Lerp(
            defaultSpotAngle,
            0,
            ropeAnimUpCurve.Evaluate(Mathf.Clamp01(animProgressRopeUp * 2))
            );

        lightSpot.range = Mathf.Lerp(
            defaultSpotRange,
            0,
            ropeAnimUpCurve.Evaluate(Mathf.Clamp01(animProgressRopeUp * 2))
            );

        lightPointA.intensity = lightPointB.intensity = Mathf.Lerp(
            defaultPointIntensity,
            0,
            ropeAnimUpCurve.Evaluate(Mathf.Clamp01(animProgressRopeUp * 2))
            );

        if (animProgressRopeUp == 1)
        {
            animProgressRopeDown = 0;
            SpawnItem(GetRandomItem(itemDict[currentItemInfo.value], currentItemInfo.ID));
        }

    }

    private void HandleRopeDown()
    {
        currentItemTrans.localPosition = Vector3.zero;

        animProgressRopeDown = Mathf.MoveTowards(animProgressRopeDown, 1, ropeAnimDownSpeed * Time.deltaTime);

        if(animProgressRopeDown > 0.5f)
        {
            lightSpot.spotAngle = lightSpot.innerSpotAngle = Mathf.Lerp(
            0,
            defaultSpotAngle,
            ropeAnimDownCurve.Evaluate(Mathf.Clamp01(animProgressRopeDown - (1 - animProgressRopeDown)))
            );

            lightSpot.range = Mathf.Lerp(
                0,
                defaultSpotRange,
                ropeAnimDownCurve.Evaluate(Mathf.Clamp01(animProgressRopeDown - (1 - animProgressRopeDown)))
                );

            lightPointA.intensity = lightPointB.intensity = Mathf.Lerp(
                0,
                defaultPointIntensity,
                ropeAnimDownCurve.Evaluate(Mathf.Clamp01(animProgressRopeDown - (1 - animProgressRopeDown)))
                );
        }

        transform.position = Vector3.Lerp(
            defaultPos + ropeOffset,
            defaultPos,
            ropeAnimDownCurve.Evaluate(animProgressRopeDown)
            );
    }

    private void SpawnItem(GameObject itemToSpawn)
    {
        Destroy(currentItemInfo.gameObject);
        currentItemInfo = Instantiate(itemToSpawn, itemHolder).GetComponent<item_base>();
        currentItemTrans = currentItemInfo.transform;
        currentItemInfo.rb.isKinematic = true;

        checkForItemTaken = true;
        //   intInfoRef.takesItem = true;
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
        yield return new WaitForSeconds(2.5f);

        SpawnItem(GetRandomItem(itemDict[itemInfo.value], itemInfo.ID));
    }

    // note: should make a variation of shotgun prefab with 0 shells attached for godhook to prevent "full shotgun farming".
    // not that important though
}
