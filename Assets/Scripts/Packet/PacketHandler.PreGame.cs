using Assets;
using Google.Protobuf.Common;
using Google.Protobuf.Protocol;
using Google.Protobuf;
using ServerCore;
using UnityEngine;

namespace DummyClient
{
    partial class PacketHandler
    {
        // 서버 연결 되었을 때
        public static void S2C_ConnectedHandler(PacketSession session, IMessage packet)
        {
            Debug.Log("Connected!");

            C2S_Login loginPacket = new C2S_Login();

            string path = Application.dataPath;
            loginPacket.AccountName = path.GetHashCode().ToString();
            Managers.NetworkMgr.Send(loginPacket);
        }

        public static void S2C_PingHandler(PacketSession session, IMessage packet)
        {
            C2S_Ping pingPacket = new C2S_Ping();
            Debug.Log("Ping!");
            Managers.NetworkMgr.Send(pingPacket);
        }

        // 로그인 + 캐릭터 목록
        public static void S2C_LoginHandler(PacketSession session, IMessage packet)
        {
            S2C_Login loginPacket = packet as S2C_Login;
            Debug.Log("Login Success!");

            // TODO : Lobby UI에서 캐릭터 보여주고, 선택할 수 있도록 만든 후, 수정 예정
            if (loginPacket.Players == null || loginPacket.Players.Count == 0)
            {
                C2S_CreatePlayer createPacket = new C2S_CreatePlayer();
                createPacket.Name = $"Player_{Random.Range(0, 10000).ToString("0000")}";
                Managers.NetworkMgr.Send(createPacket);
            }
            else
            {
                // TODO : 무조건 첫번째 로그인 -> 추후 수정 필요
                LobbyPlayerInfo info = loginPacket.Players[0];
                C2S_EnterGame enterGamePacket = new C2S_EnterGame();
                enterGamePacket.Name = info.Name;
                Managers.NetworkMgr.Send(enterGamePacket);

            }
        }

        public static void S2C_CreatePlayerHandler(PacketSession session, IMessage packet)
        {
            S2C_CreatePlayer createPlayerPacket = packet as S2C_CreatePlayer;

            if (createPlayerPacket.Player == null)
            {
                C2S_CreatePlayer req = new C2S_CreatePlayer();
                req.Name = $"Player_{Random.Range(0, 10000).ToString("00000")}";
                Managers.NetworkMgr.Send(req);
            }
            else
            {
                C2S_EnterGame enterGamePacket = new C2S_EnterGame();
                enterGamePacket.Name = createPlayerPacket.Player.Name;
                Managers.NetworkMgr.Send(enterGamePacket);
            }
        }
    }
}
