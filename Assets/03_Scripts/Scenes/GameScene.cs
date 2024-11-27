using Assets;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.SceneType.Game;

        Managers.MapMgr.LoadMap(1);

        Screen.SetResolution(640, 480, false);

        //GameObject player = Managers.ResourceMgr.Instantiate("Creatures/Player");
        //player.name = "Player";
        //Managers.ObjectMgr.Add(player);

        //for (int i = 0; i < 5; ++i)
        //{
        //    GameObject monster = Managers.ResourceMgr.Instantiate("Creatures/Monster");
        //    if (monster == null)
        //    {
        //        Debug.LogError("GameScene::Init() failed to create Monster");
        //        return;
        //    }
        //    monster.name = $"Monster_{i + 1}";

        //    // 랜덤 위치 스폰
        //    Vector3Int pos = new Vector3Int(
        //        Random.Range(Managers.MapMgr.MinX, Managers.MapMgr.MaxX),
        //        Random.Range(Managers.MapMgr.MinY, Managers.MapMgr.MaxY),
        //        0);

        //    MonsterController monsterController = monster.GetComponent<MonsterController>();
        //    monsterController.CellPos = pos;

        //    Managers.ObjectMgr.Add(monster);
        //}
    }
    public override void Clear()
    {
    }
}
