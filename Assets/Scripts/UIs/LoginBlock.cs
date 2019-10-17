using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginBlock : MonoBehaviour, ILoginBlock
{
    [SerializeField] private TextMeshProUGUI _headerUsername, _placeholder;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private string _placeholderText = "Enter your name...";

    public void Login(string someName)
    {
        if (!string.IsNullOrEmpty(_inputField.text))
        {
            GameManager.Login(_inputField.text);
            SetLoginBlock(_inputField.text);
        }
    }

    public void SetLoginBlock(string username)
    {
        _headerUsername.text = "Logged in as \n" + username;
        _inputField.text = "";
    }
}
