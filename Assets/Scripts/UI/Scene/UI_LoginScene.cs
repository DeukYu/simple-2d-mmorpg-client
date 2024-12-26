using System.Collections.Generic;
using System.Linq;
using Assets;
using Google.Protobuf.Common;
using Google.Protobuf.WebProtocol;
using UnityEngine;

public class UI_LoginScene : UI_Scene
{
    UI_LoginPopup loginPopup = null;
    public override void Init()
    {
        base.Init();

        loginPopup = Managers.UIMgr.ShowPopupUI<UI_LoginPopup>();
        if(loginPopup == null)
        {
            Debug.LogError("Failed to show login popup.");
            return;
        }
    }

    private void ShowSelectServerPopup(List<ServerInfo> serverList)
    {
        var selectServerPopup = Managers.UIMgr.ShowPopupUI<UI_SelectServerPopup>();
        selectServerPopup.SetServers(serverList);
    }

    public void OnLoginSuccess(LoginAccountRes res)
    {
        Debug.Log("Login success!");

        Managers.NetworkMgr.AccountId = res.AccountId;
        Managers.NetworkMgr.Token = res.Token;

        Managers.UIMgr.ClosePopupUI(loginPopup);
        ShowSelectServerPopup(res.ServerInfos.ToList());
    }
}
