using Gpm.Ui;
using System.Net.WebSockets;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.UI;

public class ChatData : InfiniteScrollData
{
    public string UserName;
    public string ChatLog;
}

public class ChatClientWebSocket : MonoBehaviour
{
    public TMP_InputField ChatInputField;
    public InfiniteScroll ChatScroll;

    public Button SendButton;          

    private WebSocket webSocket;

    private void Start()
    {
        // TODO : 서버 주소 입력
        //webSocket = new WebSocket("ws://chat");

        //webSocket.OnOpen += OnConnected;
        //webSocket.OnMessage += OnMessageReceived;
        //webSocket.OnClose += OnDisconnected;
        //webSocket.OnError += OnError;

        //webSocket.Connect();

        SendButton.onClick.AddListener(SendMessageToServer);

#if UNITY_EDITOR
        ChatInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
#endif
    }

    private void OnConnected(object sender, System.EventArgs e)
    {
        Debug.Log("Connected to WebSocket server.");
    }

    //private void OnMessageReceived(object sender, MessageEventArgs e)
    //{
    //    string message = e.Data;
    //    ChatData chatData = JsonUtility.FromJson<ChatData>(message);
    //    ChatScroll.InsertData(chatData);
    //}

    //private void OnDisconnected(object sender, CloseEventArgs e)
    //{
    //    Debug.Log("Disconnected from WebSocket server.");
    //}

    //private void OnError(object sender, ErrorEventArgs e)
    //{
    //    Debug.LogError("WebSocket error: " + e.Message);
    //}

    private void OnInputFieldEndEdit(string inputText)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToServer();
        }
    }

    private void SendMessageToServer()
    {
        string message = ChatInputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            // 서버 연결 되기 전 테스트용 //
            //webSocket.Send(message);
            ChatData chatData = new ChatData() { UserName = "dahye", ChatLog = ChatInputField.text };
            ChatScroll.InsertData(chatData);
            ///////////////////////////////

            ChatInputField.text = "";

            ChatInputField.Select();
            ChatInputField.ActivateInputField();
        }
    }

    private void OnDestroy()
    {
        if (webSocket != null)
        {
            //webSocket.Close();
        }
    }
}
