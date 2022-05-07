using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_monobase : MonoBehaviour
{
    [SerializeField]
    private List<nv_monosocket> socketInfoList;

    private int currentArtifactsCount;
    private int artifactsTargetCount;

    private void Awake()
    {
        artifactsTargetCount = socketInfoList.Count;
    }

    public void AddArtifact()
    {
        currentArtifactsCount++;
        if (currentArtifactsCount >= artifactsTargetCount)
        {
            for (int i = 0; i < socketInfoList.Count; i++)
            {
                socketInfoList[i].Lock();
            }

            HandleCompletion();
        }
    }

    public void RemoveArtifact()
    {
        currentArtifactsCount--;
    }

    protected virtual void HandleCompletion()
    {
        print("monobase completion");
    }
}
