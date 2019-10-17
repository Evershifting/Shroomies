using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ResourceInputBlock : MonoBehaviour, IResourceInputBlock
{
    [SerializeField] private TextMeshProUGUI _header, _placeholder;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private string _placeholderText = "Enter resource value!111";
    private string _resource;

    private void Awake()
    {
        ResetPlaceholderText();
    }

    public void Init(string resource)
    {
        _resource = resource;
        _header.text = resource.First().ToString().ToUpper() + resource.Substring(1);
    }

    public void SetResourceValue()
    {
        int value = -1;
        int.TryParse(_inputField.text, out value);
        if (value > 0)
            APIs.Instance.SetResource(_resource, value, ResetPlaceholderText);
    }

    private void ResetPlaceholderText()
    {
        _inputField.text = "";
        _placeholder.text = _placeholderText;
    }
}
