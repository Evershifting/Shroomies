using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Server : MonoBehaviour
{
    [SerializeField] private float _resourceGrowthTimer = 3f;
    [Range(0, 5f)]
    [SerializeField] private float _connectionDelay = 1f;

    private static float ConnectionDelay;
    private float _currentGrowthTimer = 0f;
    private List<string> _usernames = new List<string>();

    public static Server Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        ConnectionDelay = _connectionDelay;
    }

    private void Update()
    {
        Debug.Log("Update");
        _currentGrowthTimer += Time.deltaTime;
        if (_currentGrowthTimer >= _resourceGrowthTimer)
        {
            _currentGrowthTimer = 0;

            CheckName();
            GrowResources();
        }
    }

    private void CheckName()
    {
        if (!PlayerPrefs.GetString("Usernames").Contains(GameManager.Username))
        {
            string names = PlayerPrefs.GetString("Usernames") + GameManager.Username + "~";
            PlayerPrefs.SetString("Usernames", names);
        }
        if (_usernames.Count < PlayerPrefs.GetString("Usernames").Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries).Length)
        {
            _usernames.Clear();
            _usernames.AddRange(PlayerPrefs.GetString("Usernames").Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }

    private void GrowResources()
    {
        foreach (string name in _usernames)
        {
            foreach (string res in GameManager.Resources)
            {
                if (!PlayerPrefs.HasKey(string.Format("{0}~{1}", name, res)))
                    PlayerPrefs.SetInt(string.Format("{0}~{1}", name, res), 0);
                PlayerPrefs.SetInt(string.Format("{0}~{1}", name, res), PlayerPrefs.GetInt(string.Format("{0}~{1}", name, res)) + 1);
            }
        }
    }

    public void GetResource(string resource, Action<string> OnResponse)
    {
        StartCoroutine(GetResourceCoroutine(resource, OnResponse));
    }

    //left it public to fullfill TA requrements
    public IEnumerator GetResourceCoroutine(string resource, Action<string> OnResponse)
    {
        yield return new WaitForSeconds(_connectionDelay * Random.Range(0.8f, 1.2f)); //artificial delay to simulate latency

        if (PlayerPrefs.HasKey(string.Format("{0}~{1}", GameManager.Username, resource)))
        {
            Dictionary<string, string> body = new Dictionary<string, string>
            {
                { "resource", resource },
                { "value", PlayerPrefs.GetInt(string.Format("{0}~{1}", GameManager.Username, resource)).ToString() },
                { "username", GameManager.Username }
            };

            string serializedBody = JsonConvert.SerializeObject(body);
            OnResponse?.Invoke(serializedBody);
        }
        Debug.Log(resource);
    }

    public void SetResource(string JSONstring, Type type, Action OnResponse)
    {
        StartCoroutine(SetResourceCoroutine(JSONstring, type, OnResponse));
    }
    private IEnumerator SetResourceCoroutine(string JSONstring, Type type, Action OnResponse)
    {
        yield return new WaitForSeconds(_connectionDelay * Random.Range(0.8f, 1.2f)); //artificial delay to simulate latency

        var response = JsonConvert.DeserializeObject(JSONstring, type);
        ResourceResponse resourceResponce = response as ResourceResponse;

        if (resourceResponce != null)
        {
            PlayerPrefs.SetInt(string.Format("{0}~{1}", resourceResponce.username, resourceResponce.resource), resourceResponce.value);

            OnResponse?.Invoke();
        }
    }
}
