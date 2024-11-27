using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UIButtonInLobby : UIButtonBase
{
    protected MenuManager _menuManager;

    protected override void Start()
    {
        base.Start();
        _menuManager = FindAnyObjectByType<MenuManager>();
    }

    protected virtual void MenuButtonClickAction(MenuType menuType)
    {
        if (_menuManager == null)
            return;

        _menuManager?.ButtonClickAction.Invoke(menuType);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        DoScaleBig();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        DoScaleOrigin();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        DoScaleOrigin();
    }

    protected override void OnClickEvent() { }
}
