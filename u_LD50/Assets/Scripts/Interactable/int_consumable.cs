using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class int_consumable : MonoBehaviour
{
    [SerializeField]
    protected int currentConsumptionStep;

    [SerializeField]
    protected List<GameObject> consumptionModelSteps;


    private void Start()
    {
        consumptionModelSteps[currentConsumptionStep].SetActive(true);
    }
}
