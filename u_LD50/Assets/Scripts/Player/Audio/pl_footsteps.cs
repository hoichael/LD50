using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_footsteps : MonoBehaviour
{
    [SerializeField]
    private FMODUnity.EventReference fEventName;

    [SerializeField]
    private float intervalWalk, intervalSprint;

    private float interval;

    private float timer;

    [SerializeField]
    private Rigidbody rb;

    private FMOD.Studio.EventInstance sfxInstance;
    void Start()
    {
    //    sfxInstance = FMODUnity.RuntimeManager.CreateInstance(fEventName);
    //    StartCoroutine(Interval());
    }

    private void Update()
    {
        if(pl_state.Instance.grounded)
        {
            //   if (new Vector3(rb.velocity.x, 0, rb.velocity.y).magnitude < 0.1f) return;

            if (rb.velocity.magnitude < 3f) return;
            timer += Time.deltaTime;

            interval = pl_state.Instance.currentFovOffsetSprint > 2f ? intervalSprint : intervalWalk;

            if (timer > interval)
            {
                //sfxInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //sfxInstance.start();
                //sfxInstance.release();

                FMODUnity.RuntimeManager.PlayOneShot(fEventName);

                timer = 0;
            }
        }
    }


    /*
    private IEnumerator Interval()
    {
        sfxInstance.start();
        sfxInstance.release();

        yield return new WaitForSeconds(interval);

        sfxInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        StartCoroutine(Interval());
    }
    */
}
