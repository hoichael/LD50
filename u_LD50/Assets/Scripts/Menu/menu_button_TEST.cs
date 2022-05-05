using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_button_TEST : MonoBehaviour
{
    [SerializeField]
    private menu_main menu;

    private bool hover;

    [SerializeField]
    private MenuButtonType type;

    [SerializeField]
    private TMPro.TMP_Text text;

    [SerializeField]
    private TMPro.TextMeshProUGUI textEl;

    private Material matTest;

    private RectTransform rc;

    private Material fontMat;

    private void Start()
    {
        //   matTest = new Material(text.fontMaterial);
        fontMat = text.fontMaterial;
        rc = text.rectTransform;
    }

    private void OnMouseEnter()
    {
        print("hover enter");
        hover = true;
        fontMat.SetFloat("_GlowPower", 0.5f);
        //   text.fontSize = 32;
        rc.localScale = Vector3.one * 1.2f;
    }

    private void OnMouseExit()
    {
        print("hover exit");
        hover = false;
        fontMat.SetFloat("_GlowPower", 0f);
        //   text.fontSize = 24;
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
