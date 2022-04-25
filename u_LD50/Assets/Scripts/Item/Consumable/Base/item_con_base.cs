using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_con_base : item_base
{
    public int hpAmount;

    protected bool canUse = true;

    [SerializeField]
    protected float useCooldown;

    [SerializeField]
    protected int currentConsumptionStep;

    [SerializeField]
    protected List<GameObject> consumptionModelSteps;

    public pl_item_manager itemManager;
    public pl_health_manager healthManager;

    [Header("Eat Animation")]

    [SerializeField]
    private float animSpeed;

    [SerializeField]
    private Vector3 eatTargetPos;

    [SerializeField]
    private AnimationCurve eatAnimCurve;

    private float currentAnimProgress;

    private float currentAnimTarget, currentAnimStart;

    public override void Use()
    {
        if (!canUse) return;
        
        base.Use();

        canUse = false;
        currentAnimProgress = 0;
        currentAnimStart = 0;
        currentAnimTarget = 1;
    }

    private void Update()
    {
        if (currentAnimProgress == currentAnimTarget) return;
        HandleEatAnim();
    }

    private void HandleEatAnim()
    {
        currentAnimProgress = Mathf.MoveTowards(currentAnimProgress, currentAnimTarget, animSpeed * Time.deltaTime);

        transform.localPosition = Vector3.Lerp(
            Vector3.zero,
            eatTargetPos,
            eatAnimCurve.Evaluate(currentAnimProgress)
            );

        if(currentAnimProgress == currentAnimTarget)
        {
            if(currentAnimTarget == 1)
            {
                HandleConsumption();
                currentAnimStart = 1;
                currentAnimTarget = 0;
                currentAnimProgress = 1;
            }
            else
            {
                canUse = true;
            }
        }
    }

    protected virtual void HandleConsumption()
    {

        consumptionModelSteps[currentConsumptionStep].SetActive(false);
        currentConsumptionStep++;

        value--;
        if (value < 1) value = 1;

        pl_state.Instance.health += hpAmount;
        if (pl_state.Instance.health > pl_settings.Instance.maxHealth)
        {
            pl_state.Instance.health = pl_settings.Instance.maxHealth;
        }
        healthManager.HandleHealthChange();

        if (currentConsumptionStep == consumptionModelSteps.Count)
        {
            HandleDepletion();
        }
        else
        {
            consumptionModelSteps[currentConsumptionStep].SetActive(true);
            col = consumptionModelSteps[currentConsumptionStep].GetComponentInChildren<Collider>().gameObject;
        }
    }

    protected virtual void HandleDepletion()
    {
        itemManager.currentItemInfo = null;
        itemManager.currentlyInPickupAnim = false;
        Destroy(gameObject);
    }
}
