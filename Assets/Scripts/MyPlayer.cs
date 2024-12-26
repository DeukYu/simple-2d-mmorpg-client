using Google.Protobuf;
using Google.Protobuf.Protocol;
using System.Collections;
using UnityEngine;

public class MyPlayer : Player
{
    NetworkManager _network;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("CoSendPacket");
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CoSendPacket()
    {
        while (true)
        {
            Debug.Log("Send move packet");
            yield return new WaitForSeconds(0.25f);

            C2S_Move movePacket = new C2S_Move();
            movePacket.PosInfo.PosX = Random.Range(-50, 50);
            movePacket.PosInfo.PosY = 0;

            _network.Send(movePacket);
        }
    }
}
