using System.Collections.Generic;
using Assets;
using Google.Protobuf.Common;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectServerPopup : UI_Popup
{
    public List<UI_SelectServerPopup_Item> Items { get; } = new List<UI_SelectServerPopup_Item>();

    public override void Init()
    {

    }

    public void SetServers(List<ServerInfo> servers)
    {
        Items.Clear();

        var grid = GetComponentInChildren<GridLayoutGroup>().gameObject;
        foreach (Transform child in grid.transform)
            Destroy(child.gameObject);

        foreach (var server in servers)
        {
            var go = Managers.ResourceMgr.Instantiate("UI/Popup/UI_SelectServerPopup_Item", grid.transform);
            UI_SelectServerPopup_Item item = go.GetOrAddComponent<UI_SelectServerPopup_Item>();
            Items.Add(item);

            item.serverInfo = server;
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (Items.Count <= 0)
            return;

        foreach (var item in Items)
        {
            item.RefreshUI();
        }
    }
}