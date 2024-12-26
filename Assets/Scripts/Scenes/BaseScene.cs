using Assets;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.SceneType SceneType { get; protected set; } = Define.SceneType.Unknown;
    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        var obj = GameObject.FindFirstObjectByType(typeof(EventSystem));
        if (obj == null)
        {
            Managers.ResourceMgr.Instantiate("UI/EventSystem").name = "@EventSystem";
        }
    }

    public abstract void Clear();
}