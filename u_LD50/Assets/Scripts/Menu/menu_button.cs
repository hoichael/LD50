using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_button : MonoBehaviour
{
    [SerializeField]
    private menu_main menu;

    [SerializeField]
    private MenuButtonType type;

//    [SerializeField]
//    private TMPro.TMP_Text text;

    public menu_char_info[] charInfoArr;

    private float currentAnimProgress = 1;

    [SerializeField]
    private float animSpeed;



    private void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        for (int i = 0; i < charInfoArr.Length; i++)
        {
            charInfoArr[i].textComponent.fontMaterial.SetFloat("_GlowPower", 0.5f);
            charInfoArr[i].textComponent.rectTransform.localScale = Vector3.one * 1.2f;
        }

     //   fontMat.SetFloat("_GlowPower", 0.5f);
     //   rc.localScale = Vector3.one * 1.2f;
    }

    private void OnMouseExit()
    {
        for (int i = 0; i < charInfoArr.Length; i++)
        {
            charInfoArr[i].textComponent.fontMaterial.SetFloat("_GlowPower", 0);
            charInfoArr[i].textComponent.rectTransform.localScale = Vector3.one;
        }
        //   fontMat.SetFloat("_GlowPower", 0f);
        //   rc.localScale = Vector3.one;
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

public class menu_char_info
{
    public Transform containerTrans;
    public TMPro.TMP_Text textComponent;
    public Vector3 animStartPos, animTargetPos;
    public Quaternion animStartRot, animTargetRot;
}
