using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static APIs;
using Random = UnityEngine.Random;

public class Server : MonoBehaviour
{
    [SerializeField] private float _resourceGrowthTimer = 3f;
    [Range(0, 5f)]
    [SerializeField] private float _connectionDelay = 1f;

    private static float ConnectionDelay;
    private float currentGrowthTimer = 0f;

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
        currentGrowthTimer += Time.deltaTime;
        if (currentGrowthTimer >= _resourceGrowthTimer)
        {
            currentGrowthTimer = 0;
            foreach (string res in GameManager.Resources)
            {
                if (!PlayerPrefs.HasKey(string.Format("{0}~{1}", GameManager.username, res)))
                    PlayerPrefs.SetInt(string.Format("{0}~{1}", GameManager.username, res), 0);
                PlayerPrefs.SetInt(string.Format("{0}~{1}", GameManager.username, res), PlayerPrefs.GetInt(string.Format("{0}~{1}", GameManager.username, res)) + 1);
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

        if (PlayerPrefs.HasKey(string.Format("{0}~{1}", GameManager.username, resource)))
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("resource", resource);
            body.Add("value", PlayerPrefs.GetInt(string.Format("{0}~{1}", GameManager.username, resource)).ToString());
            body.Add("username", GameManager.username);

            string serializedBody = JsonConvert.SerializeObject(body);

            OnResponse?.Invoke(serializedBody);
        }
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
