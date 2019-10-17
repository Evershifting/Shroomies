using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceCounter : MonoBehaviour, IResourceCounter
{
    [SerializeField] private TextMeshProUGUI _resource, _value;

    public TextMeshProUGUI ResourceAmount { get => _value; set => _value = value; }

    public void Init(string resource, int value)
    {
        _resource.text = resource;
        ResourceAmount.text = value.ToString();
    }
}
