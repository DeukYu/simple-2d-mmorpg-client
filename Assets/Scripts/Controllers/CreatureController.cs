using Assets;
using Google.Protobuf.Common;
using Google.Protobuf.Enum;
using UnityEngine;

public class CreatureController : BaseController
{
    protected HpBar _hpBar;

    PositionInfo _positionInfo = new PositionInfo();
    StatInfo _statInfo = new StatInfo();
    public override StatInfo StatInfo
    {
        get { return base.StatInfo; }
        set { base.StatInfo = value; UpdateHpBar(); }
    }
    public override int Hp
    {
        get { return StatInfo.Hp; }
        set { base.Hp = value; UpdateHpBar(); }
    }

    protected void AddHpBar()
    {
        GameObject go = Managers.ResourceMgr.Instantiate("UI/HpBar", transform);
        if (go == null)
        {
            Debug.LogError("HpBar Instantiate failed.");
            return;
        }
        go.transform.localPosition = new Vector3(0, 0.5f);
        go.name = "HpBar";
        _hpBar = go.GetComponent<HpBar>();
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        if (_hpBar == null)
        {
            return;
        }

        if (_statInfo.MaxHp < 0)
        {
            return;
        }

        float ratio = ((float)Hp) / _statInfo.MaxHp;
        _hpBar.SetHpBar(ratio);
    }

    protected override void Init()
    {
        base.Init();
        AddHpBar();
    }

    public virtual void OnDamaged()
    {
    }
    public virtual void OnDead()
    {
        State = CreatureState.Dead;

        var effect = Managers.ResourceMgr.Instantiate("Effects/DieEffect");
        effect.transform.position = transform.position;
        effect.GetComponent<Animator>().Play("DieEffect");
        GameObject.Destroy(effect, 0.5f);
    }

    public virtual void UseSkill(int skillId)
    {
    }
}
