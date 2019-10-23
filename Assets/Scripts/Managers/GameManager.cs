using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<string> _resources = new List<string>() { "Shrooms", "Darts", "Hatred" };
    [SerializeField] private float _resourceCheckDelay = 5f;

    private float _resourceCheckTimer = 0f;


    public static string Username = "User";

    public static List<string> Resources { get; private set; }
    public static int Score { get; set; } = 0;

    private void Awake()
    {
        Resources = _resources;
    }

    private void Start()
    {
        UIManager.Instance.SetLoginBlock(Username);

        foreach (string resource in Resources)
        {
            UIManager.Instance.GenerateUI(resource);
        }

        _resourceCheckTimer = _resourceCheckDelay;
    }

    private void Update()
    {
        _resourceCheckTimer += Time.deltaTime;
        if (_resourceCheckTimer >= _resourceCheckDelay)
        {
            _resourceCheckTimer = 0;
            UpdateResourceValues();
        }
    }

    private static void UpdateResourceValues()
    {
        foreach (string resource in Resources)
        {
            APIs.Instance.GetResource(resource, UIManager.Instance.UpdateResource);
        }
    }
    
    public static void Login(string username)
    {
        Debug.Log(string.Format("Username is {0}", username));
        if (!string.Equals(username, GameManager.Username))
        {
            Debug.Log(username);
            GameManager.Username = username;
            UpdateResourceValues();
        }
    }
}
