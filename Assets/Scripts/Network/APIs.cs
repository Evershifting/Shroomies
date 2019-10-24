using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIs : MonoBehaviour
{
    public static APIs Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    public void GetResource(string resource, Action<string> OnResponse)
    {
        StartCoroutine(GetResourceCoroutine(resource, OnResponse));
    }
    private IEnumerator GetResourceCoroutine(string resource, Action<string> OnResponse)
    {
        Server.Instance.GetResource(resource, OnResponse);

        yield return null;
    }

    public ServerResponce GetResource(string resource)
    {        
        return Server.Instance.GetResourceValue(resource);
    }

    public void GetScore(Action<string> OnResponse, Action OnGetAllScores)
    {
        StartCoroutine(GetScoreCoroutine(OnResponse, OnGetAllScores));
    }
    private IEnumerator GetScoreCoroutine(Action<string> OnResponse, Action OnGetAllScores)
    {
        List<Coroutine> routines = new List<Coroutine>();

        for (int i = 0; i < GameManager.Resources.Count; i++)
        {
            routines.Add(StartCoroutine(Server.Instance.GetResourceCoroutine(GameManager.Resources[i], OnResponse)));
        }

        Debug.Log("Before Routine");
        foreach (Coroutine coroutine in routines)
        {
            yield return coroutine;
            Debug.Log("Routine end");
        }
        Debug.Log("After Routine");

        OnGetAllScores?.Invoke();
    }

    public void SetResource(string resource, int value, Action callback)
    {
        StartCoroutine(SetResourceCoroutine(resource, value, callback));
    }
    private IEnumerator SetResourceCoroutine(string resource, int value, Action OnResponse)
    {
        Type type = typeof(ResourceResponse);

        Dictionary<string, string> body = new Dictionary<string, string>
        {
            { "resource", resource },
            { "value", value.ToString() },
            { "username", GameManager.Username }
        };

        string serializedBody = JsonConvert.SerializeObject(body);

        Server.Instance.SetResource(serializedBody, type, OnResponse);

        yield return null; //we must wait for a server's answer in this yield 
    }
}

public class ServerResponce
{
    public bool isError;
    public object responce;

    public ServerResponce(bool isError)
    {
        this.isError = isError;
        responce = null;
    }
    public ServerResponce(object responce)
    {
        this.responce = responce;
        isError = false;
    }
}

public class ResourceResponse
{
    public string resource;
    public int value;
    public string username;

    public ResourceResponse(string resource, int value, string username)
    {
        this.resource = resource;
        this.value = value;
        this.username = username;
    }
}