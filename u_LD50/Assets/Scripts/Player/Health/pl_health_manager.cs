using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health_manager : MonoBehaviour
{

    [SerializeField]
    private pl_health_pp healthPP;

    [SerializeField]
    private pl_health_ui healthUI;

    [SerializeField]
    private pl_health_audio healthAudio;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private pl_input input;

    public Vector3 savePos = Vector3.zero;

    [SerializeField]
    private GameManager gameManager;

    public void HandleHealthChange()
    {
        UpdateOthers();
    }

    private void UpdateOthers()
    {
        healthPP.HealthChange();
        healthUI.HealthChange();
        healthAudio.HealthChange();

        if(pl_state.Instance.health <= 0)
        {
            Vector3 rebirthPos = savePos;
            savePos = Vector3.zero;
            gameManager.Death(rebirthPos);
        }
    }
}
