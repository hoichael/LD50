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

        //   buttonInfo.textArr = new TMPro.TMP_Text[charArr.Length];
        //    buttonInfo.charInfoArr = new menu_char_info[charArr.Length];
        buttonInfo.charInfoList = new List<menu_char_info>();

        for (int i = 0; i < charArr.Length; i++)
        {
            var charHolder = new GameObject();
            charHolder.name = "CharHolder";
            charHolder.transform.SetParent(container.transform);

            var tmpBase = Instantiate(charTMPbase);
            tmpBase.transform.SetParent(charHolder.transform);

            charHolder.transform.localPosition = Vector3.zero;

            TMPro.TMP_Text textEl = tmpBase.GetComponent<TMPro.TMP_Text>();
            textEl.text = charArr[i].ToString();

            buttonInfo.charInfoList.Add(new menu_char_info());
            buttonInfo.charInfoList[i].containerTrans = charHolder.transform;
            buttonInfo.charInfoList[i].textComponent = textEl;
            buttonInfo.charInfoList[i].hoverRot = Quaternion.Euler(new Vector3(menu_settings.baseRotX, 0, 0));
        }

        if(charArr.Length % 2 == 0)
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
        for (int i = 0; i < buttonInfo.charInfoList.Count; i++)
        {
            
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
