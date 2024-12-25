using Assets;
using UnityEngine;

public class LoginScene : BaseScene
{
    UI_LoginScene _sceneUI;
    protected override void Init()
    {
        base.Init();

        SceneType = Define.SceneType.Login;

        Screen.SetResolution(640, 480, false);

        _sceneUI = Managers.UIMgr.ShowSceneUI<UI_LoginScene>("LoginScene", null);

    }
    public override void Clear()
    {
    }
}
