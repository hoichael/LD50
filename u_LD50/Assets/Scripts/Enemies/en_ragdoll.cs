using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_ragdoll : MonoBehaviour
{
    [SerializeField]
    private en_info_base info;

    [SerializeField]
    private Collider col;

    private Collider[] ragDollCols;
    private Rigidbody[] ragDollRBs;

    [SerializeField]
    private en_brain_base brain;

    private void Awake()
    {
        ragDollCols = GetComponentsInChildren<Collider>();
        ragDollRBs = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool state)
    {

        for(int i = 0; i < ragDollRBs.Length; i++)
        {
            ragDollRBs[i].detectCollisions = state;
            ragDollRBs[i].isKinematic = !state;
        }

        for (int i = 0; i < ragDollCols.Length; i++)
        {
            ragDollCols[i].enabled = state;
        }

        info.anim.enabled = !state;
        info.rb.detectCollisions = !state;
        info.rb.isKinematic = state;
        col.enabled = !state;

        brain.enabled = !state;
    }

}
