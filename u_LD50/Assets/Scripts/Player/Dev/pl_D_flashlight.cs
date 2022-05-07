using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_D_flashlight : MonoBehaviour
{
    [Header("Flashlight Fields")]

    [SerializeField]
    private Light lightPrimary;

    [SerializeField]
    private Light lightSecondary;

    [SerializeField]
    private float xRotHide;

    private float xRotCurrent;

    [SerializeField]
    private float hideAnimLerpFactor;

    private float currentLerpTarget;

    public bool currentlyEnabled;

    private IEnumerator currentFlickerExec;

    [Header("Battery Handling")]

    [SerializeField]
    private float batteryUseInterval;

    [SerializeField]
    private List<Transform> batteryPosList;

    [SerializeField]
    private List<item_battery_base> batteryInfoList = new List<item_battery_base>();

    private Transform currentPickupTrans;
    private Vector3 pickupInitPos;
    private Quaternion pickupInitRot;

    [SerializeField]
    private float currentPickupProgress = 1;

    [SerializeField]
    private float pickupAnimSpeed;

    [SerializeField]
    private float pickupAnimOffsetY;

    [SerializeField]
    private AnimationCurve pickupAnimCurve;

    private int batteryCapacity;

    private bool gotJuice;

    [Header("Audio")]

    [SerializeField]
    private FMODUnity.StudioEventEmitter sfxLoopEmitter;

    [SerializeField]
    private FMODUnity.EventReference sfxOn, sfxOff;

    [SerializeField]
    private Transform sfxOriginPos;

    private void Start()
    {
        gotJuice = true;

        batteryCapacity = batteryPosList.Count;

        currentlyEnabled = lightPrimary.enabled;

        if (!currentlyEnabled)
        {
            xRotCurrent = currentLerpTarget = xRotHide;
            transform.localRotation = Quaternion.Euler(xRotCurrent, 0, 0);
        }
        else
        {
            sfxLoopEmitter.Play();
            StartCoroutine(FlickerCheck(10));
            StartCoroutine(BatteryRoutine());
        }
    }

    public bool CanPickup(item_battery_base batteryInfo)
    {
        if (batteryInfo.currentUse >= batteryInfo.totalUses) return false;

        if (batteryInfoList.Count + 1 > batteryCapacity || currentPickupTrans != null)
        {
            return false;
        }

        return true;
    }

    public void InitBatteryPickup(item_battery_base batteryInfo)
    {
        HandleBatteryPickup(batteryInfo);
    }

    private void HandleBatteryPickup(item_battery_base batteryInfo)
    {

        batteryInfoList.Add(batteryInfo);

        Destroy(batteryInfo.rb);
        batteryInfo.col.SetActive(false);    //    batteryInfo.col.enabled = false;

        batteryInfo.transform.SetParent(batteryPosList[batteryInfoList.Count - 1]);

        // init pickup anim
        currentPickupTrans = batteryInfo.transform;
        currentPickupProgress = 0;
        pickupInitPos = batteryInfo.transform.localPosition;
        pickupInitRot = batteryInfo.transform.localRotation;

        batteryInfo.currentAssociatedFlashlight = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        HandlePickupAnim();

        if (transform.localRotation.x == currentLerpTarget) return;
        HandleToggleAnim();
    }

    private void ToggleFlashlight()
    {
        currentLerpTarget = currentlyEnabled ? xRotHide : 0;
        currentlyEnabled = !currentlyEnabled;

        lightPrimary.enabled = lightSecondary.enabled = gotJuice;

        if(!currentlyEnabled)
        {
            lightPrimary.enabled = lightSecondary.enabled = currentlyEnabled;
            FMODUnity.RuntimeManager.PlayOneShot(sfxOff, sfxOriginPos.position);
            sfxLoopEmitter.Stop();
            StopAllCoroutines();
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(sfxOn, sfxOriginPos.position);
            if (gotJuice)
            {
                sfxLoopEmitter.Play();
                StartCoroutine(FlickerCheck(10));
                StartCoroutine(BatteryRoutine());
            }
        }
    }

    private void HandleToggleAnim()
    {
        xRotCurrent = Mathf.Lerp(
            xRotCurrent,
            currentLerpTarget,
            hideAnimLerpFactor * Time.deltaTime
            );

        transform.localRotation = Quaternion.Euler(xRotCurrent, 0, 0);
    }

    private void HandlePickupAnim()
    {
        if (currentPickupProgress == 1) return;

        currentPickupProgress = Mathf.MoveTowards(currentPickupProgress, 1, pickupAnimSpeed * Time.deltaTime);

        // lerp position
        currentPickupTrans.localPosition = Vector3.Lerp(
            pickupInitPos,
            Vector3.zero,
            pickupAnimCurve.Evaluate(currentPickupProgress)
            );

        // lerp and apply y pos offset
        float offsetY = Mathf.Lerp(
            0,
            pickupAnimOffsetY,
            pickupAnimCurve.Evaluate(Mathf.PingPong(currentPickupProgress, 0.5f))
            );

        currentPickupTrans.position += new Vector3(0, offsetY, 0);

        // lerp rotation
        currentPickupTrans.localRotation = Quaternion.Lerp(
            pickupInitRot,
            Quaternion.identity,
            pickupAnimCurve.Evaluate(currentPickupProgress)
            );

        if(currentPickupProgress == 1)
        {
            currentPickupTrans.localPosition = Vector3.zero;
            currentPickupTrans = null;

            lightPrimary.enabled = lightSecondary.enabled = gotJuice = true;
            sfxLoopEmitter.Play();

            if (batteryInfoList.Count == 1)
            {
                StartCoroutine(BatteryRoutine());
            }
        }
    }

    public void EjectBattery()
    {
        item_battery_base batteryInfo = batteryInfoList[batteryInfoList.Count - 1];

        batteryInfoList.Remove(batteryInfo);

        if(batteryInfoList.Count < 1)
        {
            lightPrimary.enabled = lightSecondary.enabled = gotJuice = false;
            sfxLoopEmitter.Stop();
            StopAllCoroutines();
        }

        batteryInfo.currentAssociatedFlashlight = null;

        batteryInfo.transform.SetParent(null);

        Rigidbody rb = batteryInfo.gameObject.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        batteryInfo.rb = rb;

        rb.AddForce(Vector3.up * 200, ForceMode.Force);
        rb.AddTorque(new Vector3(Random.Range(-0.9f, 0.9f), 0, Random.Range(-0.9f, 0.9f)) * 150);

        batteryInfo.type = "Prop";
        batteryInfo.tag = "Interactable";

        batteryInfo.value = 1;

        // this is still ugly
        StartCoroutine(EnableEjectedBatteryCol(batteryInfo.col));
    }

    private IEnumerator EnableEjectedBatteryCol(GameObject col)
    {
        yield return new WaitForSeconds(0.2f);
        col.SetActive(true);    // col.enabled = true;
    }

    private IEnumerator BatteryRoutine()
    {
        yield return new WaitForSeconds(batteryUseInterval);
        if (batteryInfoList.Count > 0)
        {
            batteryInfoList[batteryInfoList.Count - 1].Use();
            StartCoroutine(BatteryRoutine());
        }
    }

    // ----------------------------- FLICKER -----------------------------
    private IEnumerator FlickerCheck(float delay)
    {

        yield return new WaitForSeconds(delay);

        if(Random.Range(0, 10) > 5 && currentFlickerExec == null)
        {
            CallFlickerExec();
        }

        CallFlickerCheck();
    }

    private IEnumerator FlickerExec()
    {
        if (!gotJuice) yield break;

        // mb play sfx for each flicker?

        lightPrimary.enabled = lightSecondary.enabled = false;
        sfxLoopEmitter.Stop();

        yield return new WaitForSeconds(Random.Range(1f, 15) / 100);

        lightPrimary.enabled = lightSecondary.enabled = true;
        sfxLoopEmitter.Play();

        yield return new WaitForSeconds(Random.Range(1f, 30) / 100);

        lightPrimary.enabled = lightSecondary.enabled = false;
        sfxLoopEmitter.Stop();

        yield return new WaitForSeconds(Random.Range(1f, 15) / 100);

        lightPrimary.enabled = lightSecondary.enabled = true;
        sfxLoopEmitter.Play();

        currentFlickerExec = null;
        if (Random.Range(0, 10) > 6)
        {
            CallFlickerExec();
        }
    }

    private void CallFlickerCheck()
    {
        StartCoroutine(FlickerCheck(Random.Range(4, 20)));
    }

    private void CallFlickerExec()
    {
        currentFlickerExec = FlickerExec();
        StartCoroutine(currentFlickerExec);
    }
}
