using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_swiper_look : MonoBehaviour
{
    [SerializeField]
    private Transform lookTargetTrans;

    [SerializeField]
    private Transform plCamTrans;

    private void Start()
    {
        plCamTrans = pl_state.Instance.GLOBAL_CAM_REF.transform;
    }

    void Update()
    {
        lookTargetTrans.position = plCamTrans.position;
    }
}
