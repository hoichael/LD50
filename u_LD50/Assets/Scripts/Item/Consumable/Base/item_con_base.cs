using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_con_base : item_base
{
    public int hpAmount;

    protected bool canUse = true;

    [SerializeField]
    protected float useCooldown;

    protected IEnumerator currentCooldownRoutine;

    [SerializeField]
    protected int currentConsumptionStep;

    [SerializeField]
    protected List<GameObject> consumptionModelSteps;

    public pl_item_manager itemManager;
    public pl_health_ui healthUI;

    public override void Use()
    {
        if (!canUse) return;
        
        base.Use();

        canUse = false;
        currentCooldownRoutine = UseCooldown();
        StartCoroutine(currentCooldownRoutine);
        HandleConsumption();
    }

    protected virtual void HandleConsumption()
    {
        print("init consume. current step: " + currentConsumptionStep);

        consumptionModelSteps[currentConsumptionStep].SetActive(false);
        currentConsumptionStep++;

        pl_state.Instance.health += hpAmount;
        if (pl_state.Instance.health > pl_settings.Instance.maxHealth)
        {
            pl_state.Instance.health = pl_settings.Instance.maxHealth;
        }
        healthUI.HealthChange();

        if (currentConsumptionStep == consumptionModelSteps.Count)
        {
            HandleDepletion();
        }
        else
        {
            consumptionModelSteps[currentConsumptionStep].SetActive(true);
            col = consumptionModelSteps[currentConsumptionStep].GetComponent<Collider>();
        }
    }

    protected virtual void HandleDepletion()
    {
        itemManager.currentItemInfo = null;
        itemManager.currentlyInPickupAnim = false;
        print("consumable depleted");
        Destroy(gameObject);
    }

    protected virtual IEnumerator UseCooldown()
    {
        yield return new WaitForSeconds(useCooldown);
        canUse = true;
    }
}
