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
    private CharacterJoint[] ragDollJoints;

    [SerializeField]
    private en_brain_base brain;

    public Rigidbody rbRagRoot; // used to apply force in HandleDeath

    [SerializeField]
    private en_goat_gravity grav;

    private void Awake()
    {
        ragDollCols = GetComponentsInChildren<Collider>();
        ragDollRBs = GetComponentsInChildren<Rigidbody>();
        ragDollJoints = GetComponentsInChildren<CharacterJoint>();
    }

    private void Update()
    {
     //   if (Input.GetKeyDown(KeyCode.Q)) ToggleRagdoll(true);
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

        if (state == true)
        {
            StartCoroutine(RagdollLifetime());
        }
    }

    private IEnumerator RagdollLifetime()
    {
        yield return new WaitForSeconds(7);

        grav.enabled = false;

        for (int i = 0; i < ragDollJoints.Length; i++)
        {
            //    ragDollRBs[i].detectCollisions = false;
            //   ragDollRBs[i].isKinematic = true;
            Destroy(ragDollJoints[i]);
        }

        for (int i = 1; i < ragDollRBs.Length; i++)
        {
        //    ragDollRBs[i].detectCollisions = false;
         //   ragDollRBs[i].isKinematic = true;
            Destroy(ragDollRBs[i]);
        }

        for (int i = 1; i < ragDollCols.Length; i++)
        {
        //    ragDollCols[i].enabled = false;
            Destroy(ragDollCols[i]);
        }


        this.enabled = false;
    }

}
