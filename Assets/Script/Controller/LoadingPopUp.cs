using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPopUp : Singleton<LoadingPopUp>
{

    [SerializeField] GameObject objLoading = default;
    public void SetActive(bool isActive)
    {
        objLoading.SetActive(isActive);
    }
}
