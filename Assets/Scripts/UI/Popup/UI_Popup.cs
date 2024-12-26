using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        Managers.UIMgr.SetCanvas(gameObject, true);
    }
    public virtual void ClosePopupUI()
    {
        Managers.UIMgr.ClosePopupUI(this);
    }
}