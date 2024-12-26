using Assets;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_GameScene _sceneUI;
    protected override void Init()
    {
        base.Init();

        SceneType = Define.SceneType.Game;

        Managers.MapMgr.LoadMap(1);

        Screen.SetResolution(640, 480, false);

        _sceneUI = Managers.UIMgr.ShowSceneUI<UI_GameScene>("GameScene", null);

    }
    public override void Clear()
    {
    }
}
