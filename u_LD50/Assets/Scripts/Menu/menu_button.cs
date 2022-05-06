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
    private TMPro.TMP_Text text;

    private RectTransform rc;

    private Material fontMat;

    private void Start()
    {
        fontMat = text.fontMaterial;
        rc = text.rectTransform;
    }

    private void OnMouseEnter()
    {
        fontMat.SetFloat("_GlowPower", 0.5f);
        rc.localScale = Vector3.one * 1.2f;
    }

    private void OnMouseExit()
    {
        fontMat.SetFloat("_GlowPower", 0f);
        rc.localScale = Vector3.one;
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
