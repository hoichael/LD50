using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_main : MonoBehaviour
{

    public void HandleButtonPress(MenuButtonType type)
    {
        switch(type)
        {
            case MenuButtonType.Enter:
                EnterGame();
                break;

            case MenuButtonType.DEFAULT:
                print("oops");
                break;
        }
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("sc_main");
    }
}

[System.Serializable]
public enum MenuButtonType
{
    DEFAULT,
    Enter
}
