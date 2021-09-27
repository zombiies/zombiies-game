using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopupController : Singleton<NotificationPopupController>
{

    [SerializeField] Transform transParentElement = default;
    [SerializeField] Button btnContent = default;
    [SerializeField] Button btnClose = default;
    [SerializeField] NotificationElementUI elementUI = default;
    [SerializeField] Text txtContent = default;

    private NotificationModel[] notificationModels;

    void Awake()
    {
        OnButtonClicked();
        notificationModels = DataManager.Instance.listNotificationUser.ToArray();
    }
    private void OnEnable()
    {
        SetContent();
    }
    private void OnButtonClicked()
    {
        btnClose.onClick.AddListener(OnCloseButtonClicked);
        btnContent.onClick.AddListener(OnContentButtonClicked);
    }
    private void OnCloseButtonClicked()
    {
        GraphQLManager.Instance.MarkAllNotificationsAsRead(() =>
        {
            DataManager.Instance.listNotificationUser.ForEach(x => x.isRead = true);
        });
        if (HomeController.Instance)
            HomeController.Instance.SetReadNotification();
        gameObject.SetActive(false);
    }
    private void OnContentButtonClicked()
    {
        btnContent.gameObject.SetActive(false);
    }
    private void OnElementButtonClicked(string _txtContent)
    {
        btnContent.gameObject.SetActive(true);
        txtContent.text = _txtContent;
    }
    private void DeleteElement()
    {
        int count = transParentElement.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            Destroy(transParentElement.GetChild(i).gameObject);
        }
    }
    private void SetContent()
    {
        DeleteElement();
        for (int i = notificationModels.Length - 1; i >= 0; i--)
        {
            NotificationElementUI _elementUI = Instantiate<NotificationElementUI>(elementUI, transParentElement);
            _elementUI.SetData(notificationModels[i]);
            _elementUI.onElementClick += OnElementButtonClicked;
        }
    }

}
