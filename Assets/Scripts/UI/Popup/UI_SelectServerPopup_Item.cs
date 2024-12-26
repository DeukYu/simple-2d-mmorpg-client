using System.Diagnostics;
using Assets;
using Google.Protobuf.Common;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectServerPopup_Item : UI_Base
{
    public ServerInfo serverInfo { get; set; }

    enum Buttons
    {
        SelectServerButton
    }

    enum Texts
    {
        NameText,
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Get<Button>((int)Buttons.SelectServerButton).gameObject.BindEvent(OnClickSelectServerButton);
    }

    public void RefreshUI()
    {
        if (serverInfo == null)
            return;
        Get<Text>((int)Texts.NameText).text = serverInfo.Name;
    }

    void OnClickSelectServerButton(PointerEventData evt)
    {
        Managers.NetworkMgr.ConnectToGame(serverInfo.IpAddress, serverInfo.Port);
        Managers.SceneMgrEx.LoadScene(Define.SceneType.Game);
        Managers.UIMgr.ClosePopupUI();
    }
}