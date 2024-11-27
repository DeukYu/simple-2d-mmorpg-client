using Assets;
using Google.Protobuf.Common;
using Google.Protobuf.Enum;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public LocalPlayerController LocalPlayer { get; set; }
    Dictionary<long, GameObject> _objects = new Dictionary<long, GameObject>();

    public static GameObjectType GetObjectTypeById(long id)
    {
        var type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

    public bool Add(ObjectInfo objectInfo, bool isLocalPlayer = false)
    {
        var ojbectType = GetObjectTypeById(objectInfo.ObjectId);

        GameObject go = null;

        if (ojbectType == GameObjectType.Player)
        {
            go = isLocalPlayer ? CreateLocalPlayer(objectInfo) : CreatePlayer(objectInfo);
        }
        else if (ojbectType == GameObjectType.Monster)
        {
            go = CreateMonster(objectInfo);
        }
        else if (ojbectType == GameObjectType.Projectile)
        {
            go = CreateProjectile(objectInfo);
        }

        if (go == null)
        {
            Debug.LogError("ObjectManager::Add() failed. go is null");
            return false;
        }

        go.name = objectInfo.Name;
        _objects.Add(objectInfo.ObjectId, go);

        return true;
    }

    private GameObject CreateLocalPlayer(ObjectInfo objectInfo)
    {
        var go = Managers.ResourceMgr.Instantiate("Creatures/LocalPlayer");
        if (go == null)
        {
            Debug.Log("ObjectManager::Add() failed. go is null");
            return null;
        }

        LocalPlayer = go.GetComponent<LocalPlayerController>();
        LocalPlayer.Id = objectInfo.ObjectId;
        LocalPlayer.PositionInfo = objectInfo.PosInfo;
        LocalPlayer.StatInfo = objectInfo.StatInfo;
        LocalPlayer.SyncPos();

        return go;
    }

    private GameObject CreatePlayer(ObjectInfo objectInfo)
    {
        var go = Managers.ResourceMgr.Instantiate("Creatures/Player");
        if (go == null)
        {
            Debug.Log("ObjectManager::Add() failed. go is null");
            return null;
        }

        var pc = go.GetComponent<PlayerController>();
        pc.Id = objectInfo.ObjectId;
        pc.PositionInfo = objectInfo.PosInfo;
        pc.StatInfo = objectInfo.StatInfo;
        pc.SyncPos();

        return go;
    }

    private GameObject CreateMonster(ObjectInfo objectInfo)
    {
        var go = Managers.ResourceMgr.Instantiate("Creatures/Monster");
        if (go == null)
        {
            Debug.Log("ObjectManager::Add() failed. go is null");
            return null;
        }

        var mc = go.GetComponent<MonsterController>();
        mc.Id = objectInfo.ObjectId;
        mc.PositionInfo = objectInfo.PosInfo;
        mc.StatInfo = objectInfo.StatInfo;
        mc.SyncPos();

        return go;
    }

    private GameObject CreateProjectile(ObjectInfo objectInfo)
    {
        var go = Managers.ResourceMgr.Instantiate("Creatures/Arrow");
        if(go == null)
        {
            Debug.LogError("ObjectManager::Add() failed. go is null");
            return null;
        }

        ArrowController ac = go.GetComponent<ArrowController>();
        ac.PositionInfo = objectInfo.PosInfo;
        ac.StatInfo = objectInfo.StatInfo;
        ac.SyncPos();

        return go;
    }

    public void Remove(long playerId)
    {
        var go = FindById(playerId);
        if (go == null)
            return;

        _objects.Remove(playerId);
        Managers.ResourceMgr.Destroy(go);
    }

    public void RemoveLocalPlayer()
    {
        if (LocalPlayer == null)
            return;

        Remove(LocalPlayer.Id);
        LocalPlayer = null;
    }

    public GameObject FindById(long id)
    {
        if (_objects.TryGetValue(id, out GameObject go))
        {
            return go;
        }

        return null;
    }

    public GameObject FindCreature(Vector3Int cellPos)
    {
        foreach (var obj in _objects.Values)
        {
            var cc = obj.GetComponent<CreatureController>();
            if (cc == null)
                continue;

            if (cc.CellPos == cellPos)
            {
                return obj;
            }
        }

        return null;
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        foreach (var obj in _objects.Values)
        {
            if (condition.Invoke(obj))
            {
                return obj;
            }
        }

        return null;
    }
    public void Clear()
    {
        foreach (var obj in _objects.Values)
        {
            Managers.ResourceMgr.Destroy(obj);
        }

        _objects.Clear();
        LocalPlayer = null;
    }
}
