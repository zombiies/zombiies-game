using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationElementUI : MonoBehaviour
{
    [SerializeField] Button btnElement = default;
    [SerializeField] Text txtContent = default;
    [SerializeField] Image imgElement = default;
    private string content = "";
    public Action<string> onElementClick = null;
    private void Awake()
    {
        OnButtonClicked();
    }
    private void OnButtonClicked()
    {
        btnElement.onClick.AddListener(OnElementButtonClicked);
    }
    private void OnElementButtonClicked()
    {
        if (onElementClick != null) onElementClick(content);
    }
    public void SetData(NotificationModel notification)
    {
        content = notification.content;
        SetTextContent(notification.content);
        if (notification.isRead)
        {
            imgElement.color = new Color32(0, 0, 0, 50);
            txtContent.fontStyle = FontStyle.Normal;
        }
        else
        {
            imgElement.color = new Color32(0, 0, 0, 100);
            txtContent.fontStyle = FontStyle.Bold;
        }
    }
    private void SetTextContent(string _txtContent)
    {
        if (_txtContent.Length > 30)
        {
            _txtContent = _txtContent.Substring(0, 30) + "...";
        }
        txtContent.text = _txtContent;
    }
}
