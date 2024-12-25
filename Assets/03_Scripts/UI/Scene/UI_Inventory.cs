using Assets;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Inventory : UI_Base
{
    public List<UI_Inventory_Item> Items { get; } = new List<UI_Inventory_Item>();
    public override void Init()
    {
        Items.Clear();

        var grid = transform.Find("ItemGrid").gameObject;
        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 20; ++i)
        {
            var go = Managers.ResourceMgr.Instantiate("UI/Scene/UI_Inventory_Item", grid.transform);
            UI_Inventory_Item item = go.GetOrAddComponent<UI_Inventory_Item>();
            Items.Add(item);
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        if(Items.Count <= 0)
        {
            return;
        }

        List<Item> items = Managers.InventoryMgr.Items.Values.ToList();
        items.Sort((left, right) => { return left.Slot - right.Slot; });

        foreach (var item in items)
        {
            if (item.Slot < 0 || item.Slot >= 20)
            {
                Debug.LogError($"Invalid slot number {item.Slot}");
                continue;
            }

            Items[item.Slot].SetItem(item);
        }
    }
}
