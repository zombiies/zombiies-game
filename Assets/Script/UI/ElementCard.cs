using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementCard : MonoBehaviour
{
    public Button btnElement = default;
    public CardInformation cardInformation = default;
    public CardInformationUI cardInformationUI = default;
    public Text txt_Amount = default;

    void Start()
    {
        btnElement.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
    }
}
