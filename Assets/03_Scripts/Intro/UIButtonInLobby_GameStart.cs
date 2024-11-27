using UnityEngine;

public class UIButtonInLobby_GameStart : UIButtonInLobby
{
    protected override void OnClickEvent()
    {
        MenuButtonClickAction(Define.MenuType.GameStart);
    }
}
