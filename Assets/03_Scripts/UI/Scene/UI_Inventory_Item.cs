using Assets;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory_Item : UI_Base
{
    [SerializeField]
    Image _icon = null;

    [SerializeField]
    Image _frame = null;

    public long ItemUid { get; private set; }
    public int ItemId { get; private set; }
    public int Count { get; private set; }
    public bool Equipped { get; private set; }
    public override void Init()
    {
        _icon.gameObject.BindEvent((e) =>
        {
            Debug.Log("Click");

            if(Managers.DataMgr.ItemDataDict.TryGetValue(ItemId, out var itemData) == false)
            {
                return;
            }

            // 아이템 사용 패킷
            if (itemData.ItemType == ItemType.Consumable)
                return;

            C2S_EquipItem equipItemPacket = new C2S_EquipItem
            {
                ItemUid = ItemUid,
                Equipped = Equipped
            };
            Managers.NetworkMgr.Send(equipItemPacket);
        });
    }

    public void SetItem(Item item)
    {
        if(item == null)
        {
            ItemUid = 0;
            ItemId = 0;
            Count = 0;
            Equipped = false;

            _icon.gameObject.SetActive(false);
            _frame.gameObject.SetActive(false);
        }
        else
        {
            ItemUid = item.ItemUid;
            ItemId = item.ItemId;
            Count = item.Count;
            Equipped = item.Equipped;

            if (Managers.DataMgr.ItemDataDict.TryGetValue(item.ItemId, out var itemData))
            {
                return;
            }

            var icon = Managers.ResourceMgr.Load<Sprite>($"Textures/Items/{itemData.IconPath}");
            _icon.sprite = icon;

            _icon.gameObject.SetActive(true);
            _frame.gameObject.SetActive(Equipped);
        }  
    }
}
