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
    protected int currentConsumptionStep;

    [SerializeField]
    protected List<GameObject> consumptionModelSteps;

    protected virtual void Start()
    {
        currentConsumptionStep = consumptionModelSteps.Count - 1;
    }

    public override void Use()
    {
        if (!canUse) return;
        
        base.Use();

        if(currentConsumptionStep > 1)
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

        consumptionModelSteps[currentConsumptionStep].SetActive(false);
        currentConsumptionStep--;
        consumptionModelSteps[currentConsumptionStep].SetActive(true);
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
