using System.Collections.Generic;
using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<long, Item> Items = new Dictionary<long, Item>();

    public void Add(Item item)
    {
        Items.Add(item.ItemUid, item);
    }

    public bool TryGet(long itemUid, out Item item)
    {
        return Items.TryGetValue(itemUid, out item);
    }

    public Item Find(Func<Item, bool> condition)
    {
        foreach (Item item in Items.Values)
        {
            if (condition.Invoke(item))
            {
                return item;
            }
        }
        return null;
    }
    public void Clear()
    {
        Items.Clear();
    }
}
