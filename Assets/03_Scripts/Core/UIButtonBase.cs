using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    protected Button _button;
    protected RectTransform _buttonRT;
    protected Vector3 _originScale;

    protected virtual void Start()
    {
        _buttonRT = GetComponent<RectTransform>();
        _button = gameObject.GetOrAddComponent<Button>();

        _button.onClick.AddListener(OnClickEvent);
        _originScale = _buttonRT.localScale;
    }

    public abstract void OnPointerDown(PointerEventData eventData);

    public abstract void OnPointerEnter(PointerEventData eventData);

    public abstract void OnPointerExit(PointerEventData eventData);

    protected abstract void OnClickEvent();

    protected void DoScaleOrigin()
    {
        _buttonRT.DOScale(_originScale, 0.05f);
    }

    protected void DoScaleBig(float multiplier = 1.2f)
    {
        _buttonRT.DOScale(_originScale * multiplier, 0.05f);
    }
}
