using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ed_menu_gen : EditorWindow
{
    private GameObject buttonContainer;
    private GameObject charTMPbase;
    private string text;
    private char[] charArr;

    private menu_button buttonInfo;

    [MenuItem("Custom/GenerateButton")]
    public static void OpenWindow()
    {
        GetWindow(typeof(ed_menu_gen));
    }

    private void OnGUI()
    {
        buttonContainer = EditorGUILayout.ObjectField("Button Container", buttonContainer, typeof(GameObject), true) as GameObject;
        GUILayout.Space(40);

        charTMPbase = EditorGUILayout.ObjectField("TMP Base Prefab", charTMPbase, typeof(GameObject), false) as GameObject;
        GUILayout.Space(40);

        text = EditorGUILayout.TextField("Button Text", text);
        GUILayout.Space(40);

        if (GUILayout.Button("Execute"))
        {
            Init();
        }
    }

    private void Init()
    {
        if (buttonContainer == null || text.Length == 0 || charTMPbase == null) return;

        charArr = text.ToCharArray();

        if (buttonContainer.transform.Find("CharacterContainer") != null)
        {
            DestroyImmediate(buttonContainer.transform.Find("CharacterContainer").gameObject);
        }

        var container = new GameObject();
        container.name = "CharacterContainer";
        container.transform.SetParent(buttonContainer.transform);
        container.transform.localPosition = Vector3.zero;

        buttonInfo = buttonContainer.GetComponent<menu_button>();
        buttonInfo.charInfoList = new List<menu_char_info>();

        for (int i = 0; i < charArr.Length; i++)
        {
            buttonInfo.charInfoList.Add(new menu_char_info());

            var charHolder = new GameObject();
            charHolder.name = "CharHolder";
            charHolder.transform.SetParent(container.transform);
            charHolder.transform.localPosition = Vector3.zero;

            charHolder.AddComponent<menu_char>();
            buttonInfo.charInfoList[i].charAnim = charHolder.GetComponent<menu_char>();

            var tmpBase = Instantiate(charTMPbase);
            tmpBase.transform.SetParent(charHolder.transform);

            TMPro.TMP_Text textEl = tmpBase.GetComponent<TMPro.TMP_Text>();
            textEl.text = charArr[i].ToString();
            textEl.rectTransform.localPosition = Vector3.zero;
            textEl.fontSize = menu_settings.fontSize;

            buttonInfo.charInfoList[i].containerTrans = charHolder.transform;
            buttonInfo.charInfoList[i].textComponent = textEl;
        }

        if (charArr.Length % 2 == 0)
        {
            PositionCharsEven();
        }
        else
        {
            PositionCharsOdd();
        }

        container.transform.localRotation = Quaternion.Euler(new Vector3(menu_settings.baseRotX, 0, 0));
    }

    private void PositionCharsEven()
    {
        for (int i = 0; i < buttonInfo.charInfoList.Count / 2; i++)
        {
            Vector3 pos;
            Vector3 hoverPos;
            if(i == 0)
            {
                pos = new Vector3(menu_settings.charSpacingDefault / 2, 0, 0);
                hoverPos = new Vector3(menu_settings.charSpacingHover / 2, 0, 0);
            }
            else
            {
                pos = new Vector3(menu_settings.charSpacingDefault * i + menu_settings.charSpacingDefault / 2, 0, 0);
                hoverPos = new Vector3(menu_settings.charSpacingHover * i + menu_settings.charSpacingHover / 2, 0, 0);
            }

            buttonInfo.charInfoList[Mathf.RoundToInt(((float)buttonInfo.charInfoList.Count - 1) / 2 + i + 0.5f)].containerTrans.localPosition = pos;
            buttonInfo.charInfoList[Mathf.RoundToInt(((float)buttonInfo.charInfoList.Count - 1) / 2 + i + 0.5f)].hoverPos = hoverPos;

            buttonInfo.charInfoList[Mathf.RoundToInt(((float)buttonInfo.charInfoList.Count - 1) / 2 - i - 0.5f)].containerTrans.localPosition = -pos;
            buttonInfo.charInfoList[Mathf.RoundToInt(((float)buttonInfo.charInfoList.Count - 1) / 2 - i - 0.5f)].hoverPos = -hoverPos;
        }
    }

    private void PositionCharsOdd()
    {
        for (int i = 1; i <= (buttonInfo.charInfoList.Count - 1) / 2; i++)
        {
            buttonInfo.charInfoList[(buttonInfo.charInfoList.Count - 1) / 2 + i].containerTrans.localPosition = new Vector3((menu_settings.charSpacingDefault * i), 0, 0);
            buttonInfo.charInfoList[(buttonInfo.charInfoList.Count - 1) / 2 + i].hoverPos = new Vector3((menu_settings.charSpacingHover * i), 0, 0);

            buttonInfo.charInfoList[(buttonInfo.charInfoList.Count - 1) / 2 - i].containerTrans.localPosition = new Vector3(-(menu_settings.charSpacingDefault * i), 0, 0);
            buttonInfo.charInfoList[(buttonInfo.charInfoList.Count - 1) / 2 - i].hoverPos = new Vector3(-(menu_settings.charSpacingHover * i), 0, 0);
        }
    }
}
