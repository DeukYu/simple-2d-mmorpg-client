using Assets;

public class UI_Scene : UI_Base
{
    public override void Init()
    {
        Managers.UIMgr.SetCanvas(gameObject, false);
    }
}
