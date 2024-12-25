using DummyClient;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

class PacketManager
{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance { get { return _instance; } }
    #endregion

    PacketManager()
    {
        Register();
    }

    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
    Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
    private Dictionary<Type, ushort> _typeToMsgId = new Dictionary<Type, ushort>();

    public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }

    public void Register()
    {
        // ���� ��������� IMessage�� ������ ���߻� Ÿ�� ��������
        var packetTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(IMessage).IsAssignableFrom(x) && !x.IsAbstract);

        foreach (var packetType in packetTypes)
        {
            // Descriptor�� ��������, null üũ
            var descriptor = GetMessageDescriptor(packetType);
            if (descriptor == null)
                continue;

            // �޽��� �̸����� �޽��� ID ���
            ushort messageId = ComputeMessageId(descriptor.Name);
            if (_onRecv.ContainsKey(messageId))
            {
                Debug.Log($"Already registered message: {messageId}");
                continue;
            }

            // MakePacket<T>�� ȣ���ϴ� ��������Ʈ ����
            var makePacketAction = CreateMakePacketAction(packetType);

            // ��������Ʈ �� �ڵ鷯 ���
            _onRecv.Add(messageId, makePacketAction);
            RegisterHandler(messageId, packetType);
            _typeToMsgId.Add(packetType, messageId);
        }
    }

    public ushort GetMessageId(Type packetType)
    {
        _typeToMsgId.TryGetValue(packetType, out ushort id);
        return id;
    }

    // �޽��� ID ��� �޼���
    private void RegisterHandler(ushort messageId, Type packetType)
    {
        var handler = PacketHandler.GetHandler(packetType);
        if (handler != null)
        {
            _handler[messageId] = handler;
        }
    }

    // Descriptor�� �������� �޼���
    private MessageDescriptor GetMessageDescriptor(Type packetType)
    {
        var descriptor = packetType.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static)?
                                    .GetValue(null) as MessageDescriptor;
        if (descriptor == null)
        {
            Debug.Log($"Descriptor not found for packet type: {packetType.Name}");
            return null;
        }

        return descriptor;
    }

    // MakePacket ȣ���� ���� ��������Ʈ ���� �޼���
    private Action<PacketSession, ArraySegment<byte>, ushort> CreateMakePacketAction(Type packetType)
    {
        return (session, buffer, id) =>
        {
            MethodInfo method = typeof(PacketManager).GetMethod(nameof(MakePacket), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo genericMethod = method.MakeGenericMethod(packetType);
            genericMethod.Invoke(this, new object[] { session, buffer, id });
        };
    }

    private ushort ComputeMessageId(string messageName)
    {
        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(messageName));
        return BitConverter.ToUInt16(hash, 0);
    }

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Action<PacketSession, ArraySegment<byte>, ushort> action = null;
        if (_onRecv.TryGetValue(id, out action))
            action.Invoke(session, buffer, id);
    }

    void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
    {
        T pkt = new T();
        pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

        if (CustomHandler != null)
        {
            CustomHandler.Invoke(session, pkt, id);
        }
        else
        {
            Action<PacketSession, IMessage> action = null;
            if (_handler.TryGetValue(id, out action))
                action.Invoke(session, pkt);
        }
    }

    public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
    {
        Action<PacketSession, IMessage> action = null;
        if (_handler.TryGetValue(id, out action))
            return action;
        return null;
    }
}