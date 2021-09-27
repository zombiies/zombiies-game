using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class WaitForReadyPopupController : MonoBehaviour
{

    [SerializeField] Button btnAccept = default;
    [SerializeField] Button btnDecline = default;
    [SerializeField] Image imgLoading = default;
    private bool isStop = false;
    private Tween tween;
    void Start()
    {
        OnButtonClicked();
    }
    private void OnEnable()
    {
        ResetLoading();
    }
    private void ResetLoading()
    {
        isStop = false;
        SetInteractable(true);
        tween = imgLoading.DOFillAmount(0f, 30f)
            .OnUpdate(() =>
            {
                if (isStop)
                    tween.Kill();
            });
    }
    void OnButtonClicked()
    {
        btnAccept.onClick.AddListener(OnAcceptButtonClicked);
        btnDecline.onClick.AddListener(OnDeclineButtonClicked);
    }
    void OnDeclineButtonClicked()
    {
        SetInteractable(false);
        isStop = true;
    }
    void OnAcceptButtonClicked()
    {
        GraphQLManager.Instance.ReadyMatch(() =>
        {
            SetInteractable(false);
            isStop = true;
        });
    }
    private void SetInteractable(bool isActive)
    {
        if (btnAccept)
            btnAccept.interactable = isActive;
        if (btnDecline)
            btnDecline.interactable = isActive;
    }

}
