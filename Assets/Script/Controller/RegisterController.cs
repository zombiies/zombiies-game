using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterController : MonoBehaviour
{
    [SerializeField] GameObject objLogin;
    [Header("Button")]
    [SerializeField] Button btnRegister = default;
    [SerializeField] Button btnLogin = default;
    [Header("TEXT INPUT")]
    [SerializeField] InputField txtInEmail = default;
    [SerializeField] InputField txtInputUsername = default;
    [SerializeField] InputField txtInPassword = default;
    [SerializeField] Text txtError = default;
    // Start is called before the first frame update
    void Start()
    {
        OnClickedButton();
    }
    private void OnEnable()
    {
        txtInEmail.text = "";
        txtInPassword.text = "";
        txtInputUsername.text = "";
    }
    private void OnClickedButton()
    {
        btnLogin.onClick.AddListener(OnLoginButtonClicked);
        btnRegister.onClick.AddListener(OnRegisterButtonClicked);
    }

    private void OnRegisterButtonClicked()
    {
        GraphQLManager.Instance.Register(txtInputUsername.text, txtInEmail.text, txtInPassword.text, txtError);
    }

    private void OnLoginButtonClicked()
    {
        objLogin.SetActive(true);
        gameObject.SetActive(false);
    }
}
