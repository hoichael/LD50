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

    public TMPro.TMP_Text[] textArr;

    private float currentAnimProgress = 1;

    [SerializeField]
    private float animSpeed;



    private void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        for (int i = 0; i < textArr.Length; i++)
        {
            textArr[i].fontMaterial.SetFloat("_GlowPower", 0.5f);
            textArr[i].rectTransform.localScale = Vector3.one * 1.2f;
        }

     //   fontMat.SetFloat("_GlowPower", 0.5f);
     //   rc.localScale = Vector3.one * 1.2f;
    }

    private void OnMouseExit()
    {
        for (int i = 0; i < textArr.Length; i++)
        {
            textArr[i].fontMaterial.SetFloat("_GlowPower", 0);
            textArr[i].rectTransform.localScale = Vector3.one;
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
