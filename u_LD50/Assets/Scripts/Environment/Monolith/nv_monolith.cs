using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv_monolith : nv_monobase
{/*
    [SerializeField]
    private List<nv_monosocket> socketInfoList;

    private int currentArtifactsCount;
    private int artifactsTargetCount;

    [SerializeField]
    private pl_health_damage plDmg;

    [SerializeField]
    private dmg_base dmgInfo;

    private void Awake()
    {
        artifactsTargetCount = socketInfoList.Count;
    }

    public void AddArtifact()
    {
        currentArtifactsCount++;
        if (currentArtifactsCount >= artifactsTargetCount)
        {
            for(int i = 0; i < socketInfoList.Count; i++)
            {
                socketInfoList[i].Lock();
            }

            InitTheEnd();
        }
    }

    public void RemoveArtifact()
    {
        currentArtifactsCount--;
    }

    private void InitTheEnd()
    {
        print("dun duuuuun");
        plDmg.HandleDamage(dmgInfo);
    }
    */
    [SerializeField]
    private pl_health_damage plDmg;

    [SerializeField]
    private dmg_base dmgInfo;

    protected override void HandleCompletion()
    {
        base.HandleCompletion();
        print("dun duuuuun");
        plDmg.HandleDamage(dmgInfo);
    }
}
