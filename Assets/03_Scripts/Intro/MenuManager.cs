using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Action<Define.MenuType> ButtonClickAction;

    private void Start()
    {
        ButtonClickAction += ButtonClickHandler;
    }

    private void ButtonClickHandler(Define.MenuType menuType)
    {
        SetUI(menuType);
    }

    private void SetUI(Define.MenuType menuType)
    {
        switch (menuType)
        {
            case Define.MenuType.GameStart:
                GameStart();
                break;
            case Define.MenuType.Option:
                Option();
                break;
            case Define.MenuType.Exit:
                Exit();
                break;
        }
    }

    private void GameStart()
    {
        Debug.Log("GameStart~!");
    }
    
    private void Option()
    {
        Debug.Log("Option~!");
    }

    private void Exit()
    {
        Debug.Log("Exit~!");
    }
}
