using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindFirstObjectByType<BaseScene>(); } }
    public void LoadScene(Define.SceneType type)
    {
        Managers.Clear();
        CurrentScene.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }
    string GetSceneName(Define.SceneType type)
    {
        return System.Enum.GetName(typeof(Define.SceneType), type);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}