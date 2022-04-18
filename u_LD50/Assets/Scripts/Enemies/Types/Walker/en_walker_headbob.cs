using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_walker_headbob : MonoBehaviour
{
    [SerializeField]
    private en_walker_info info;

    [SerializeField]
    private Transform chestTarget;

    [SerializeField]
    private Transform chestDefault;

    [SerializeField]
    private float frequencyIdle, amplitudeIdle, frequencyWalk, amplitudeWalk;

    [SerializeField]
    private float frequency, amplitude;

    private void OnEnable()
    {
        UpdateValues();
    }

    public void UpdateValues()
    {
        if (info.currentlyWalking)
        {
            frequency = frequencyWalk;
            amplitude = amplitudeWalk;
        }
        else
        {
            frequency = frequencyIdle;
            amplitude = amplitudeIdle;
        }
    }

    private void Update()
    {
        Vector3 newPos = Vector3.zero;

        newPos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude;
        newPos.y += Mathf.Sin(Time.time * frequency) * amplitude;

        chestTarget.position = chestDefault.position + newPos;
    }

    private void OnDisable()
    {
        
    }
}
