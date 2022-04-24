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
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if(savePos != Vector3.zero) // kinda ugly but vector3 isnt nullable and it works as player can never be at 000 world pos due to terrain layout
        {
            StartCoroutine(HandleRebirth());
        }
        else
        {
            gameManager.Death();
        }
    }

    private IEnumerator HandleRebirth()
    {
        yield return new WaitForSeconds(1.2f);

        transform.position = savePos;
        savePos = Vector3.zero;
        rb.isKinematic = false;
        input.enabled = true;

        pl_state.Instance.health = pl_settings.Instance.maxHealth;

        UpdateOthers();
    }

}
