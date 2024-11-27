using Assets;
using Google.Protobuf.Common;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System.Collections;
using UnityEngine;

public class LocalPlayerController : PlayerController
{
    bool _moveKeyPressed = false;
    Coroutine _coSkillCooltime;

    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        switch (State)
        {
            case CreatureState.Idle:
                GetDirInput();
                break;
            case CreatureState.Move:
                GetDirInput();
                break;
        }
        base.UpdateController();
    }

    protected override void UpdateIdle()
    {
        // 이동 상태로 갈지 확인
        if (_moveKeyPressed)
        {
            State = CreatureState.Move;
            return;
        }

        // 스킬 상태로 갈지 확인
        if (_coSkillCooltime != null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            C2S_Skill skillPacket = new C2S_Skill
            {
                SkillInfo = new SkillInfo
                {
                    SkillId = 1,
                }
            };
            Managers.NetworkMgr.Send(skillPacket);

            _coSkillCooltime = StartCoroutine(CoInputCooltime(0.2f));
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            C2S_Skill skillPacket = new C2S_Skill
            {
                SkillInfo = new SkillInfo
                {
                    SkillId = 2,
                }
            };
            Managers.NetworkMgr.Send(skillPacket);

            _coSkillCooltime = StartCoroutine(CoInputCooltime(0.2f));
        }
    }

    IEnumerator CoInputCooltime(float time)
    {
        yield return new WaitForSeconds(time);
        _coSkillCooltime = null;
    }

    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void GetDirInput()
    {
        _moveKeyPressed = true;
        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            _moveKeyPressed = false;
        }
    }

    protected override void MoveToNextPos()
    {
        if (_moveKeyPressed == false)
        {
            State = CreatureState.Idle;
            CheckUpdatedFlag();
            return;
        }

        Vector3Int destPos = CellPos;

        switch (Dir)
        {
            case MoveDir.Up:
                if (Input.GetKey(KeyCode.W))
                    destPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                if (Input.GetKey(KeyCode.S))
                    destPos += Vector3Int.down;
                break;
            case MoveDir.Left:
                if (Input.GetKey(KeyCode.A))
                    destPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                if (Input.GetKey(KeyCode.D))
                    destPos += Vector3Int.right;
                break;
        }

        if (Managers.MapMgr.CanGo(destPos))
        {
            if (Managers.ObjectMgr.FindCreature(destPos) == null)
            {
                CellPos = destPos;
            }
        }

        CheckUpdatedFlag();
    }

    protected override void CheckUpdatedFlag()
    {
        if (_updated)
        {
            C2S_Move movePacket = new C2S_Move
            {
                PosInfo = PositionInfo
            };
            Managers.NetworkMgr.Send(movePacket);
            _updated = false;
        }
    }
}
