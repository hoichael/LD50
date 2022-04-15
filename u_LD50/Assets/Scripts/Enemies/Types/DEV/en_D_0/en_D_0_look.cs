using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_D_0_look : MonoBehaviour
{
    [SerializeField]
    private Transform trans;
    private void Update()
    {
        trans.LookAt(new Vector3(pl_state.Instance.GLOBAL_PL_TRANS_REF.position.x, 0, pl_state.Instance.GLOBAL_PL_TRANS_REF.position.z));
    }
}
