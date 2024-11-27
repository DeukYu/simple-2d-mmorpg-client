using UnityEngine;

public class UIButtonInLobby_Option : UIButtonInLobby
{
    protected override void OnClickEvent()
    {
        MenuButtonClickAction(Define.MenuType.Option);
    }
}
