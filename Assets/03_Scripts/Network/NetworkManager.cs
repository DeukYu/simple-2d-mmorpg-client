using DummyClient;
using Google.Protobuf;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;

public class NetworkManager
{
    ServerSession _session = new ServerSession();
    public long AccountId { get; set; }
    public string Token { get; set; }
    public void Send(IMessage packet)
    {
        _session.Send(packet);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ConnectToGame(string ipAddress, int port)
    {
        // DNS (Domain Name System)
        IPAddress ipAddr = IPAddress.Parse(ipAddress);
        IPEndPoint endPoint = new IPEndPoint(ipAddr, port);

        Connector connector = new Connector();

        connector.Connect(endPoint, () => { return _session; }, 1);
    }

    // Update is called once per frame
    public void Update()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (var packet in list)
        {
            Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.Id);
            if (handler != null)
                handler.Invoke(_session, packet.Message);
        }
    }
}
