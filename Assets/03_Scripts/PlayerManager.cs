using Google.Protobuf.Protocol;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    MyPlayer _myPlayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static PlayerManager Instance { get; } = new PlayerManager();

    //public void Add(S2C_PlayerList packet)
    //{
    //    Debug.Log(packet.ToString());
    //    Object obj = Resources.Load("Player");
    //    if (obj == null)
    //    {
    //        Debug.Log("Player prefab is not found");
    //        return;
    //    }

    //    Debug.Log($"Players {packet.Players.Count}");
    //    foreach (var p in packet.Players)
    //    {
    //        GameObject go = Object.Instantiate(obj) as GameObject;

    //        if (p.IsSelf)
    //        {
    //            var myPlayer = go.AddComponent<MyPlayer>();
    //            myPlayer.PlayerId = p.PlayerId;
    //            myPlayer.transform.position = new Vector3(p.PosX, p.PosY, p.PosZ);
    //            _myPlayer = myPlayer;
    //            Debug.Log("Add MyPlayer");
    //        }
    //        else
    //        {
    //            var player = go.AddComponent<Player>();
    //            player.transform.position = new Vector3(p.PosX, p.PosY, p.PosZ);
    //            _players.Add(p.PlayerId, player);
    //            Debug.Log("Add Player");
    //        }
    //    }
    //}

    //public void EnterGame(S2C_BroadcastEnterGame packet)
    //{
    //    if (packet.PlayerId == _myPlayer.PlayerId)
    //    {
    //        Debug.Log("Already entered game");
    //        return;
    //    }

    //    Object obj = Resources.Load("Player");
    //    if (obj == null)
    //    {
    //        Debug.Log("Player prefab is not found");
    //        return;
    //    }

    //    GameObject go = Object.Instantiate(obj) as GameObject;
    //    Player player = go.AddComponent<Player>();
    //    player.transform.position = new Vector3(packet.PosX, packet.PosY, packet.PosZ);
    //    _players.Add(packet.PlayerId, player);
    //}

    //public void LeaveGame(S2C_BroadcastLeaveGame packet)
    //{
    //    if (_myPlayer.PlayerId == packet.PlayerId)
    //    {
    //        GameObject.Destroy(_myPlayer.gameObject);
    //        _myPlayer = null;
    //    }
    //    else
    //    {
    //        Player player = null;
    //        if (_players.TryGetValue(packet.PlayerId, out player))
    //        {
    //            Object.Destroy(player.gameObject);
    //            _players.Remove(packet.PlayerId);
    //        }
    //    }
    //}

    //public void Move(S2C_BroadcastMove packet)
    //{
    //    if (_myPlayer == null)
    //    {
    //        Debug.Log(packet.PlayerId + " is not found");
    //        return;
    //    }

    //    if (_myPlayer.PlayerId == packet.PlayerId)
    //    {
    //        _myPlayer.transform.position = new Vector3(packet.PosX, packet.PosY, packet.PosZ);
    //    }
    //    else
    //    {
    //        Player player = null;
    //        if (_players.TryGetValue(packet.PlayerId, out player))
    //        {
    //            player.transform.position = new Vector3(packet.PosX, packet.PosY, packet.PosZ);
    //        }
    //    }
    //}
}
