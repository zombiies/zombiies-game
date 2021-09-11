using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    [SerializeField] GameObject objRegister = default;
    [SerializeField] GameObject objTick = default;
    [Header("BUTTON")]
    [SerializeField] Button btnRegister = default;
    [SerializeField] Button btnLogin = default;
    [SerializeField] Button btnRemember = default;
    [Header("TEXT INPUT")]
    [SerializeField] InputField txtInEmail = default;
    [SerializeField] InputField txtInPassword = default;
    [SerializeField] Text txtError = default;

    // Start is called before the first frame update
    void Start()
    {
        ShowWhenStart();
        OnClickedButton();
    }
    private void OnClickedButton()
    {
        btnLogin.onClick.AddListener(OnLoginButtonClicked);
        btnRegister.onClick.AddListener(OnRegisterButtonClicked);
        btnRemember.onClick.AddListener(OnRememberButttonClicked);
    }
    private void OnLoginButtonClicked()
    {
        if (DataManager.Instance.IsRemember)
            SaveInputField(txtInEmail.text, txtInPassword.text);
        else
            SaveInputField("", "");

        GraphQLManager.Instance.Login(txtInEmail.text, txtInPassword.text, txtError);
    }
    private void OnRegisterButtonClicked()
    {
        objRegister.SetActive(true);
        gameObject.SetActive(false);
    }
    private void OnRememberButttonClicked()
    {
        DataManager.Instance.IsRemember = !DataManager.Instance.IsRemember;
        objTick.SetActive(DataManager.Instance.IsRemember);
    }
    private void SaveInputField(string _inputEmail, string _inputPassword)
    {
        DataManager.Instance.InputEmail = _inputEmail;
        DataManager.Instance.InputPassword = _inputPassword;
    }
    private void ShowWhenStart()
    {
        if (DataManager.Instance.IsRemember)
        {
            objTick.SetActive(true);
            txtInEmail.text = DataManager.Instance.InputEmail;
            txtInPassword.text = DataManager.Instance.InputPassword;
        }
        else
        {
            objTick.SetActive(false);
        }
    }



}
