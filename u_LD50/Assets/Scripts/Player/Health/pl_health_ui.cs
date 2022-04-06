using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health_ui : MonoBehaviour
{
    [SerializeField]
    private Transform healthBarTrans;

    private float healthBarDefaultScaleX;
    private float healthBarDefaultScaleY;
    private float healthBarDefaultScaleZ;

    private void Start()
    {
        healthBarDefaultScaleX = healthBarTrans.localScale.x;
        healthBarDefaultScaleY = healthBarTrans.localScale.y;
        healthBarDefaultScaleZ = healthBarTrans.localScale.z;
    }

    public void HealthChange()
    {
        healthBarTrans.localScale = new Vector3(pl_state.Instance.health * (healthBarDefaultScaleX / 1000), 
            healthBarDefaultScaleY, healthBarDefaultScaleZ);
    }
}
