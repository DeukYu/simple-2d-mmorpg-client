using System;
using System.Linq;
using Assets;
using Google.Protobuf.Enum;
using Google.Protobuf.WebProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LoginScene : UI_Scene
{
    enum GameObjects
    {
        AccountName,
        Password,
    }

    enum Images
    {
        LoginButton,
        CreateButton,
    }
    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        BindButtonEvent(Images.LoginButton, OnClickLoginButton);
        BindButtonEvent(Images.CreateButton, OnClickCreateButton);
    }

    private void BindButtonEvent(Images image, Action<PointerEventData> handler)
    {
        GetImage((int)image).gameObject.BindEvent(handler);
    }

    private string GetInputFieldText(GameObjects gameObject)
    {
        var inputField = Get<GameObject>((int)gameObject).GetComponent<InputField>();
        if (inputField == null)
        {
            Debug.LogError($"InputField for {gameObject} is null.");
            return string.Empty;
        }

        return inputField.text;
    }

    private void ClearInputFields()
    {
        foreach(var field in Enum.GetValues(typeof(GameObjects)))
        {
            var inputField = Get<GameObject>((int)field)?.GetComponent<InputField>();
            if (inputField == null)
            {
                Debug.LogError($"InputField for {field} is null.");
                continue;
            }
            inputField.text = string.Empty;
        }
    }

    public void OnClickLoginButton(PointerEventData evt)
    {
        Debug.Log("OnClickLoginButton");

        string account = GetInputFieldText(GameObjects.AccountName);
        string password = GetInputFieldText(GameObjects.Password);

        LoginAccountReq req = new LoginAccountReq
        {
            AccountName = account,
            Password = password
        };

        Managers.WebMgr.SendPostRequest<LoginAccountReq, LoginAccountRes>(
            "account/login",
            req,
            res: (res) =>
            {
                if (res.Result != (int)ErrorType.Success)
                {
                    Debug.LogError($"Login Failed. ErrorType : {res.Result}");
                    return;
                }

                ClearInputFields();

                Managers.NetworkMgr.AccountId = res.AccountId;
                Managers.NetworkMgr.Token = res.Token;

                var popup = Managers.UIMgr.ShowPopupUI<UI_SelectServerPopup>();
                popup.SetServers(res.ServerInfos.ToList());
            },
            onError: (error) =>
            {
                Debug.LogError($"Login Failed: {error}");
            }
            );
    }

    public void OnClickCreateButton(PointerEventData evt)
    {
        Debug.Log("OnClickCreateButton");

        string account = GetInputFieldText(GameObjects.AccountName);
        string password = GetInputFieldText(GameObjects.Password);

        CreateAccountReq req = new CreateAccountReq
        {
            AccountName = account,
            Password = password
        };

        Managers.WebMgr.SendPostRequest<CreateAccountReq, CreateAccountRes>(
            "account/login",
            req,
            res: (res) =>
            {
                Debug.Log(res);
                ClearInputFields();
            },
            onError: (error) =>
            {
                Debug.LogError($"Login Failed: {error}");
            }
            );
    }
}
