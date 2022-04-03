using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_con_base : item_base
{
    public int hpAmount;

    protected bool canUse;

    [SerializeField]
    protected float useCooldown;

    protected IEnumerator currentCooldownRoutine;

    [SerializeField]
    protected int consumptionStepsAmount;

    [SerializeField]
    protected int currentConsumptionStep;

    [SerializeField]
    protected List<GameObject> consumptionModelSteps;

    protected virtual void Start()
    {
        currentConsumptionStep = consumptionStepsAmount;
    }

    public override void Use()
    {
        if (!canUse) return;
        
        base.Use();

        currentConsumptionStep--;

        if(currentConsumptionStep > 0)
        {
            canUse = false;
            currentCooldownRoutine = UseCooldown();
            StartCoroutine(currentCooldownRoutine);
            HandleConsumption();
        }
        else
        {
            HandleDepletion();
        }
    }

    protected virtual void HandleConsumption()
    {
        print("init consume. current step: " + currentConsumptionStep);
    }

    protected virtual void HandleDepletion()
    {
        print("consumable depleted");
    }

    protected virtual IEnumerator UseCooldown()
    {
        yield return new WaitForSeconds(useCooldown);
        canUse = true;
    }
}
