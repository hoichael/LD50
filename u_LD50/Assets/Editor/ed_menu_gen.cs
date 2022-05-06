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

    private int charSpacing = 1;

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

        menu_button buttonInfo = buttonContainer.GetComponent<menu_button>();
        buttonInfo.textArr = new TMPro.TMP_Text[charArr.Length];

        for (int i = 0; i < charArr.Length; i++)
        {
            var charHolder = new GameObject();
            charHolder.name = "CharHolder";
            charHolder.transform.SetParent(container.transform);

            var tmpBase = Instantiate(charTMPbase);
            tmpBase.transform.SetParent(charHolder.transform);

            charHolder.transform.localPosition = new Vector3(i * charSpacing, 0, 0);

            TMPro.TMP_Text textEl = tmpBase.GetComponent<TMPro.TMP_Text>();
            textEl.text = charArr[i].ToString();

            buttonInfo.textArr[i] = textEl;
        }
    }
}
