using UnityEngine;

public class UIButtonInLobby_Exit : UIButtonInLobby
{
    protected override void OnClickEvent()
    {
        MenuButtonClickAction(Define.MenuType.Exit);
    }
}
