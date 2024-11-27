using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    static SceneChanger _instance;
    public static SceneChanger Instance {  get { return _instance; } }

    private void Awake()
    {
        if(SceneChanger._instance == null)
        {
            SceneChanger._instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GoLobbyScene()
    {
        SceneManager.LoadScene("01.Lobby");
    }
}
