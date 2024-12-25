using Assets;
using Google.Protobuf.Enum;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : UI_Base
{
    enum Images
    {
        Slot_Helmet,
        Slot_Armor,
        Slot_Boots,
        Slot_Weapon,
        Slot_Shield,
    }
    
    enum Texts
    {
        AttackValueText,
        DefenseValueText,
    }

    bool _init = false;
    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<Image>(typeof(Texts));

        _init = true;   
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        Get<Image>((int)Images.Slot_Helmet).enabled = false;
        Get<Image>((int)Images.Slot_Armor).enabled = false;
        Get<Image>((int)Images.Slot_Boots).enabled = false;
        Get<Image>((int)Images.Slot_Weapon).enabled = false;
        Get<Image>((int)Images.Slot_Shield).enabled = false;

        foreach (var item in Managers.InventoryMgr.Items.Values)
        {
            if (item.Equipped == false)
                continue;

            Managers.DataMgr.ItemDataDict.TryGetValue(item.ItemId, out var itemData);

            var icon = Managers.ResourceMgr.Load<Sprite>(itemData.IconPath);

            if (item.ItemType == ItemType.Weapon)
            {
                Get<Image>((int)Images.Slot_Weapon).sprite = icon;
                Get<Image>((int)Images.Slot_Weapon).enabled = true;
            }
            else if (item.ItemType == ItemType.Armor)
            {
                var armor = (Armor)item;
                switch(armor.ArmorType)
                {
                    case ArmorType.Helmet:
                        Get<Image>((int)Images.Slot_Helmet).sprite = icon;
                        Get<Image>((int)Images.Slot_Helmet).enabled = true;
                        break;
                    case ArmorType.Armor:
                        Get<Image>((int)Images.Slot_Armor).sprite = icon;
                        Get<Image>((int)Images.Slot_Armor).enabled = true;
                        break;
                    case ArmorType.Boots:
                        Get<Image>((int)Images.Slot_Boots).sprite = icon;
                        Get<Image>((int)Images.Slot_Boots).enabled = true;
                        break;
                }
            }
        }

        var player = Managers.ObjectMgr.LocalPlayer;
        player.RefreshAdditionalStat();

        Get<Text>((int)Texts.AttackValueText).text = $"{player.StatInfo.Attack} (+ {player.WeaponDamage})";
        Get<Text>((int)Texts.DefenseValueText).text = $"{0 + player.ArmorDefense}";
    }
}
