using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_button : MonoBehaviour
{
    [SerializeField]
    private menu_main menu;

    [SerializeField]
    private MenuButtonType type;

    [SerializeField]
    public List<menu_char_info> charInfoList;

    private float currentAnimProgress = 1;

    [SerializeField]
    private float animSpeed;

    [SerializeField]
    private AnimationCurve animCurve;

    private void Update()
    {
        if(currentAnimProgress != 1)
        {
            HandleHoverAnim();
        }
    }

    private void HandleHoverAnim()
    {
        currentAnimProgress = Mathf.MoveTowards(currentAnimProgress, 1, animSpeed * Time.deltaTime);

        for(int i = 0; i < charInfoList.Count; i++)
        {
            charInfoList[i].containerTrans.localPosition = Vector3.Lerp(
                charInfoList[i].animStartPos,
                charInfoList[i].animTargetPos,
                animCurve.Evaluate(currentAnimProgress)
                );

            charInfoList[i].containerTrans.localRotation = Quaternion.Slerp(
                charInfoList[i].animStartRot,
                charInfoList[i].animTargetRot,
                animCurve.Evaluate(currentAnimProgress)
                );
        }

        /*
        if(currentAnimProgress == 1 && charInfoList[0].animStartPos == charInfoList[0].hoverPos)
        {
            for (int i = 0; i < charInfoList.Count; i++)
            {
                charInfoList[i].charAnim.pauseAnim = false;
            }
        }
        */
    }

    private void OnMouseEnter()
    {
        for (int i = 0; i < charInfoList.Count; i++)
        {
            charInfoList[i].animStartPos = charInfoList[i].containerTrans.localPosition;
            charInfoList[i].animTargetPos = charInfoList[i].hoverPos;

            charInfoList[i].animStartRot = charInfoList[i].containerTrans.localRotation;
            charInfoList[i].animTargetRot = Quaternion.Euler(Vector3.zero);

            charInfoList[i].charAnim.pauseAnim = true;
        }

        currentAnimProgress = 0;
    }

    private void OnMouseExit()
    {
        for (int i = 0; i < charInfoList.Count; i++)
        {
            /*
            charInfoList[i].animTargetPos = charInfoList[i].animStartPos;
            charInfoList[i].animStartPos = charInfoList[i].hoverPos;

            charInfoList[i].animTargetRot = charInfoList[i].animStartRot;
            charInfoList[i].animStartRot = Quaternion.Euler(Vector3.zero);
            */
            charInfoList[i].charAnim.pauseAnim = false;
        }

        /*
        if(currentAnimProgress != 1)
        {
            currentAnimProgress = 1 - currentAnimProgress;
        }
        else
        {
            currentAnimProgress = 0;
        }
        */
    }

    private void OnMouseDown()
    {
        InitClick();
    }

    public void InitClick()
    {
        print("click!");
        menu.HandleButtonPress(type);
    }
}

[System.Serializable]
public class menu_char_info
{
    public Transform containerTrans;
    public TMPro.TMP_Text textComponent;
    public Vector3 animStartPos, animTargetPos;
    public Quaternion animStartRot, animTargetRot;
    public Vector3 hoverPos;
    public menu_char charAnim;
//    public Quaternion hoverRot;
}
