using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Reflection;
using System;
using UnityEngine;
using Assets;

namespace DummyClient
{
    partial class PacketHandler
    {
        public static Action<PacketSession, IMessage> GetHandler(Type packetType)
        {
            // 핸들러 이름 생성 규칙에 맞춰 이름 설정 (예: 타입 이름 + "Handler")
            string handlerMethodName = $"{packetType.Name}Handler";

            // 현재 클래스(PacketHandler)에서 해당 이름을 가진 메서드를 찾음
            var methodInfo = typeof(PacketHandler).GetMethod(handlerMethodName,
                                BindingFlags.Public | BindingFlags.Static);

            // 핸들러가 없으면 null 반환
            if (methodInfo == null)
                return null;

            // 메서드 정보를 Action<PacketSession, IMessage> 델리게이트로 변환
            return (Action<PacketSession, IMessage>)Delegate.CreateDelegate(
                typeof(Action<PacketSession, IMessage>), methodInfo);
        }

        public static void S2C_ItemListHandler(PacketSession session, IMessage packet)
        {
            S2C_ItemList itemListPacket = packet as S2C_ItemList;

            Managers.InventoryMgr.Clear();

            // 메모리 아이템 정보 적용
            foreach (var itemInfo in itemListPacket.Items)
            {
                if (Item.MakeItem(itemInfo, out var item))
                    Managers.InventoryMgr.Add(item);
            }
            if (Managers.ObjectMgr.LocalPlayer != null)
                Managers.ObjectMgr.LocalPlayer.RefreshAdditionalStat();
        }

        public static void S2C_AddItemHandler(PacketSession session, IMessage packet)
        {
            S2C_AddItem addItemPacket = (S2C_AddItem)packet;

            foreach (var itemInfo in addItemPacket.Items)
            {
                if (Item.MakeItem(itemInfo, out var item))
                    Managers.InventoryMgr.Add(item);
            }

            Debug.Log("아이템 획득하였습니다.");

            var gameSceneUI = Managers.UIMgr.SceneUI as UI_GameScene;
            var invenUI = gameSceneUI.InvenUI;
            var statUI = gameSceneUI.StatUI;
            invenUI.RefreshUI();
            statUI.RefreshUI();

            if (Managers.ObjectMgr.LocalPlayer != null)
                Managers.ObjectMgr.LocalPlayer.RefreshAdditionalStat();
        }

        public static void S2C_EquipItemHandler(PacketSession session, IMessage packet)
        {
            S2C_EquipItem equipItemPacket = (S2C_EquipItem)packet;

            if(Managers.InventoryMgr.TryGet(equipItemPacket.ItemUid, out Item item) == false)
            {
                Debug.LogError("Failed to find item");
                return;
            }

            item.Equipped = equipItemPacket.Equipped;

            var gameSceneUI = Managers.UIMgr.SceneUI as UI_GameScene;
            var invenUI = gameSceneUI.InvenUI;
            var statUI = gameSceneUI.StatUI;
            invenUI.RefreshUI();
            statUI.RefreshUI();

            if (Managers.ObjectMgr.LocalPlayer != null)
                Managers.ObjectMgr.LocalPlayer.RefreshAdditionalStat();
        }

        public static void S2C_EnterGameHandler(PacketSession session, IMessage packet)
        {
            S2C_EnterGame enterPacket = packet as S2C_EnterGame;
            Managers.ObjectMgr.Add(enterPacket.ObjectInfo, true);
        }

        public static void S2C_LeaveGameHandler(PacketSession session, IMessage packet)
        {
            S2C_LeaveGame leavePacket = packet as S2C_LeaveGame;
            Managers.ObjectMgr.Clear();
        }

        public static void S2C_SpawnHandler(PacketSession session, IMessage packet)
        {
            S2C_Spawn spawnPacket = packet as S2C_Spawn;

            Debug.Log("Spawn");

            foreach (var player in spawnPacket.Objects)
            {
                Managers.ObjectMgr.Add(player, false);
            }
        }

        public static void S2C_DespawnHandler(PacketSession session, IMessage packet)
        {
            S2C_Despawn despawnPacket = packet as S2C_Despawn;
            ServerSession serverSession = session as ServerSession;

            foreach (var id in despawnPacket.ObjectIds)
            {
                Managers.ObjectMgr.Remove(id);
            }
        }

        public static void S2C_MoveHandler(PacketSession session, IMessage packet)
        {
            S2C_Move movePacket = packet as S2C_Move;
            ServerSession serverSession = session as ServerSession;

            var go = Managers.ObjectMgr.FindById(movePacket.ObjectId);
            if (go == null)
            {
                Debug.Log("Player not found");
                return;
            }

            if (Managers.ObjectMgr.LocalPlayer.Id == movePacket.ObjectId)
                return;

            BaseController bc = go.GetComponent<BaseController>();
            if (bc == null)
            {
                Debug.Log("Player has no BaseController");
                return;
            }

            bc.PositionInfo = movePacket.PosInfo;
        }

        public static void S2C_SkillHandler(PacketSession session, IMessage packet)
        {
            S2C_Skill skillPacket = packet as S2C_Skill;
            ServerSession serverSession = session as ServerSession;

            GameObject go = Managers.ObjectMgr.FindById(skillPacket.ObjectId);
            if (go == null)
            {
                Debug.LogError("Player not found.");
                return;
            }
            var cc = go.GetComponent<CreatureController>();
            if (cc == null)
            {
                Debug.LogError("Player has no CreatureController");
                return;
            }

            cc.UseSkill(skillPacket.SkillInfo.SkillId);
        }

        public static void S2C_ChangeHpHandler(PacketSession session, IMessage packet)
        {
            S2C_ChangeHp changeHpPacket = packet as S2C_ChangeHp;
            ServerSession serverSession = session as ServerSession;
            GameObject go = Managers.ObjectMgr.FindById(changeHpPacket.ObjectId);
            if (go == null)
            {
                Debug.LogError("Player not found.");
                return;
            }
            CreatureController cc = go.GetComponent<CreatureController>();
            if (cc == null)
            {
                Debug.LogError("Player has no CreatureController");
                return;
            }
            cc.Hp = changeHpPacket.Hp;
        }

        public static void S2C_DeadHandler(PacketSession session, IMessage packet)
        {
            S2C_Dead deadPacket = packet as S2C_Dead;
            ServerSession serverSession = session as ServerSession;
            GameObject go = Managers.ObjectMgr.FindById(deadPacket.ObjectId);
            if (go == null)
            {
                Debug.LogError("Player not found.");
                return;
            }
            CreatureController cc = go.GetComponent<CreatureController>();
            if (cc == null)
            {
                Debug.LogError("Player has no CreatureController");
                return;
            }
            cc.Hp = 0;
            cc.OnDead();
        }

        public static void S2C_ChatHandler(PacketSession session, IMessage packet)
        {
            S2C_Chat chatPacket = packet as S2C_Chat;

            // TODO : 여기서 채팅 메시지를 받아서 UI에 표시
        }

        public static void S2C_ChangeStatHandler(PacketSession session, IMessage packet)
        {
            S2C_ChangeStat changeStatPacket = (S2C_ChangeStat)packet;
        }
    }
}
