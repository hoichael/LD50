using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_footsteps : MonoBehaviour
{
    [SerializeField]
    private FMODUnity.EventReference fEventName;

    [SerializeField]
    private float interval;

    private FMOD.Studio.EventInstance sfxInstance;
    void Start()
    {
        sfxInstance = FMODUnity.RuntimeManager.CreateInstance(fEventName);
        StartCoroutine(Interval());
    }

    private IEnumerator Interval()
    {
        sfxInstance.start();

        yield return new WaitForSeconds(interval);

        //    sfxInstance.release();
        sfxInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        StartCoroutine(Interval());
    }
}
