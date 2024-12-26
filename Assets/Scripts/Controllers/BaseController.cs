using Assets;
using Google.Protobuf.Common;
using Google.Protobuf.Enum;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public long Id { get; set; }

    protected bool _updated = false;
    protected Animator _animator;
    protected SpriteRenderer _sprite;

    PositionInfo _positionInfo = new PositionInfo();
    StatInfo _statInfo = new StatInfo();
    public virtual StatInfo StatInfo
    {
        get { return _statInfo; }
        set
        {
            if (_statInfo.Equals(value))
                return;

            _statInfo.MergeFrom(value);
        }
    }

    public float Speed
    {
        get { return _statInfo.Speed; }
        set
        { _statInfo.Speed = value; }
    }
    public virtual int Hp
    {
        get { return _statInfo.Hp; }
        set
        {
            _statInfo.Hp = value;
        }
    }

    public PositionInfo PositionInfo
    {
        get { return _positionInfo; }
        set
        {
            if (_positionInfo.Equals(value))
                return;

            CellPos = new Vector3Int(value.PosX, value.PosY, 0);
            State = value.State;
            Dir = value.MoveDir;
        }
    }

    public void SyncPos()
    {
        Vector3 destPos = Managers.MapMgr.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
        transform.position = destPos;
    }

    public Vector3Int CellPos
    {
        get
        {
            return new Vector3Int(_positionInfo.PosX, _positionInfo.PosY, 0);
        }
        set
        {
            if (PositionInfo.PosX == value.x && PositionInfo.PosY == value.y)
                return;

            PositionInfo.PosX = value.x;
            PositionInfo.PosY = value.y;
            _updated = true;
        }
    }

    public virtual CreatureState State
    {
        get { return PositionInfo.State; }
        set
        {
            if (PositionInfo.State == value)
                return;

            PositionInfo.State = value;
            UpdateAnimation();
            _updated = true;
        }
    }

    public MoveDir Dir
    {
        get { return PositionInfo.MoveDir; }
        set
        {
            if (PositionInfo.MoveDir == value)
                return;

            PositionInfo.MoveDir = value;

            UpdateAnimation();
            _updated = true;
        }
    }

    public MoveDir GetDirFromVec(Vector3Int dir)
    {
        if (dir.x != 0)
            return dir.x > 0 ? MoveDir.Right : MoveDir.Left;
        else if (dir.y != 0)
            return dir.y > 0 ? MoveDir.Up : MoveDir.Down;
        return MoveDir.Down;
    }

    public Vector3Int GetFrontCellPos()
    {
        Vector3Int cellPos = CellPos;
        switch (Dir)
        {
            case MoveDir.Up:
                cellPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                cellPos += Vector3Int.down;
                break;
            case MoveDir.Left:
                cellPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                cellPos += Vector3Int.right;
                break;
        }
        return cellPos;
    }

    protected virtual void UpdateAnimation()
    {
        if(_animator == null || _sprite == null) return;

        if (State == CreatureState.Idle)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    _animator.Play("IDLE_BACK");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play("IDLE_FRONT");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Left:

                    _animator.Play("IDLE_RIGHT");
                    _sprite.flipX = true;

                    break;
                case MoveDir.Right:
                    _animator.Play("IDLE_RIGHT");
                    _sprite.flipX = false;
                    break;
            }
        }
        else if (State == CreatureState.Move)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    _animator.Play("WALK_BACK");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play("WALK_FRONT");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play("WALK_RIGHT");
                    _sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play("WALK_RIGHT");
                    _sprite.flipX = false;
                    break;
            }
        }
        else if (State == CreatureState.Skill)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    _animator.Play("ATTACK_NORMAL_BACK");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play("ATTACK_NORMAL_FRONT");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play("ATTACK_NORMAL_RIGHT");
                    _sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play("ATTACK_NORMAL_RIGHT");
                    _sprite.flipX = false;
                    break;
            }
        }
        else
        {

        }
    }

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        Vector3 pos = Managers.MapMgr.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
        transform.position = pos;
        UpdateAnimation();
    }

    protected virtual void UpdateController()
    {
        switch (State)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Move:
                UpdateMove();
                break;
            case CreatureState.Skill:
                UpdateSkill();
                break;
            case CreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    protected virtual void UpdateIdle()
    {

    }
    protected virtual void UpdateMove()
    {
        Vector3 destPos = Managers.MapMgr.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
        Vector3 moveDir = destPos - transform.position;

        // 도착 여부 체크
        float dist = moveDir.magnitude;
        if (dist < Speed * Time.deltaTime)
        {
            transform.position = destPos;
            MoveToNextPos();
        }
        else
        {
            transform.position += moveDir.normalized * Speed * Time.deltaTime;
            State = CreatureState.Move;
        }
    }

    protected virtual void MoveToNextPos()
    {

    }

    protected virtual void UpdateSkill()
    {

    }

    protected virtual void UpdateDead()
    {

    }
}
