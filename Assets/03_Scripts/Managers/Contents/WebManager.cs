using Assets;
using Google.Protobuf;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager
{
    public string BaseUrl { get; set; } = "http://localhost:5248";

    public void SendPostRequest<TRequest, TResponse>(string url, TRequest req, Action<TResponse> res, Action<string> onError = null)
    where TRequest : IMessage
    where TResponse : IMessage<TResponse>, new()
    {
        Managers.Instance.StartCoroutine(CoSendWebRequest(url, UnityWebRequest.kHttpVerbPOST, req, res, onError));
    }

    private IEnumerator CoSendWebRequest<TRequest, TResponse>(string url, string method, TRequest req, Action<TResponse> res, Action<string> onError)
    where TRequest : IMessage
    where TResponse : IMessage<TResponse>, new()
    {
        string sendUrl = $"{BaseUrl}/{url}";

        using (var uwr = new UnityWebRequest(sendUrl, method))
        {
            uwr.uploadHandler = new UploadHandlerRaw(req.ToByteArray());
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/x-protobuf");
            uwr.SetRequestHeader("Accept", "application/x-protobuf");

            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"[WebManager] Error: {uwr.error}, HTTP Status: {uwr.responseCode}");
                onError?.Invoke($"Error: {uwr.error}, HTTP Status: {uwr.responseCode}");
            }
            else
            {
                try
                {
                    if (uwr.downloadHandler.data == null || uwr.downloadHandler.data.Length == 0)
                    {
                        Debug.LogError("[WebManager] Response data is empty or null.");
                        onError?.Invoke("Response data is empty or null.");
                        yield break;
                    }

                    Debug.Log($"Response Raw Data (as Bytes): {BitConverter.ToString(uwr.downloadHandler.data)}");

                    TResponse responseData = new TResponse();
                    responseData.MergeFrom(uwr.downloadHandler.data);
                    res?.Invoke(responseData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[WebManager] Failed to parse response: {e.Message}");
                    onError?.Invoke($"Failed to parse response: {e.Message}");
                }
            }
        }
    }
}
