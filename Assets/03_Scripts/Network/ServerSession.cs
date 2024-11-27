using Google.Protobuf;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Net;
using System.Text;
using UnityEngine;

namespace DummyClient
{
    class ServerSession : PacketSession
    {
        public void Send(IMessage packet)
        {
            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];

            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));

            ushort protocolId = PacketManager.Instance.GetMessageId(packet.GetType());
            Array.Copy(BitConverter.GetBytes(protocolId), 0, sendBuffer, 2, sizeof(ushort));

            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
            Send(new ArraySegment<byte>(sendBuffer));
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Debug.Log($"OnConnected: {endPoint}");

            PacketManager.Instance.CustomHandler = (s, m, id) =>
            {
                PacketQueue.Instance.Push(id, m);
            };
        }
        public override void OnDisConnected(EndPoint endPoint)
        {
            Debug.Log($"OnDisConnected: {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
        }
    }
}