using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCardUI : MonoBehaviour
{

    [SerializeField] GameObject objProhibit = default;
    [SerializeField] Text txtAmount = default;
    
    public void SetProhibit(bool isProhibit)
    {
        if(objProhibit)
            objProhibit.SetActive(isProhibit);
    }
    public void SetText(int _number)
    {
        txtAmount.text = _number.ToString();
    }
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
