using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogInManager : MonoBehaviour
{
    public GameObject LoginUI;
    public GameObject MenuUI;

    public TMP_InputField IdField;
    public Button GoGameButton;

    private void Awake()
    {
        LogInUION();

        //IdField.onValueChanged.AddListener(IdFieldValueChanged);
        GoGameButton.onClick.AddListener(LoginButtonClick);
    }

    private void Start()
    {
        GetSavedId();
    }

    private void IdFieldValueChanged(string text)
    {
        GoGameButton.interactable = text.Length > 0;
    }

    private void LoginButtonClick()
    {
        if (PlayerPrefs.HasKey("Id"))
        {
            if (IdField.text != PlayerPrefs.GetString("Id"))
            {
                PlayerPrefs.SetString("Id", IdField.text);
            }
        }
        else
        {
            PlayerPrefs.SetString("Id", IdField.text);
        }

        ChangeScene();
    }

    private void GetSavedId()
    {
        if(PlayerPrefs.HasKey("Id"))
        {
            IdField.text = PlayerPrefs.GetString("Id");
        }
        else
        {
            IdField.text = string.Empty;
            GoGameButton.interactable = false;
        }
    }

    private void LogInUION()
    {
        LoginUI.SetActive(true);
        MenuUI.SetActive(false);
    }

    private void LogInUIOFF()
    {
        LoginUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    private void ChangeScene()
    {
        LogInUIOFF();
        //SceneChanger.Instance.GoLobbyScene();
    }
}
