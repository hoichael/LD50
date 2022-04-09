using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class en_state_base : MonoBehaviour
{
    public string stateID;

    [SerializeField]
    private en_brain_base brain;

    [SerializeField]
    protected en_info_base info;

    protected virtual void OnEnable()
    {
        print("ENEMY ENTER STATE: " + stateID);
    }

    protected virtual void OnDisable()
    {
        print("ENEMY EXIT STATE: " + stateID);
    }

    protected virtual void ChangeState(string newState)
    {
        brain.ChangeState(newState);
    }
}
