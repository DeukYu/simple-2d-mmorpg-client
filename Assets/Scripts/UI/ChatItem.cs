using Gpm.Ui;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : InfiniteScrollItem
{
    public TMP_Text ChatLogText;
    public Image ChatLogBackground;

    private ChatData _cachedChatData;
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        _cachedChatData = (ChatData)scrollData;
        ChatLogText.text += $"<color=orange>{_cachedChatData.UserName}</color> : <color=black>{_cachedChatData.ChatLog}</color>\n";

        adjustChatLogBackground();
    }

    private void adjustChatLogBackground()
    {
        if (ChatLogText != null && ChatLogBackground != null)
        {
            Vector2 textSize = ChatLogText.GetPreferredValues();
            ChatLogBackground.rectTransform.sizeDelta = new Vector2(ChatLogBackground.rectTransform.sizeDelta.x, textSize.y);
        }
    }
}
