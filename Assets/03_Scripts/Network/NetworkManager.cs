using DummyClient;
using Google.Protobuf;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;

public class NetworkManager
{
    ServerSession _session = new ServerSession();
    public void Send(IMessage packet)
    {
        _session.Send(packet);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        // DNS (Domain Name System)
        IPAddress ipAddr = DnsUtil.GetLocalIpAddress();
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

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
