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

    public void GetResource(string resource, Action<ResourceResponse> OnResponse)
    {
        StartCoroutine(GetResourceCoroutine(resource, OnResponse));
    }
    private IEnumerator GetResourceCoroutine(string resource, Action<ResourceResponse> OnResponse)
    {
        ServerResponce responce = Server.Instance.GetResourceValue(resource);
        yield return responce;
        UnfoldResourceAnswer(OnResponse, responce, resource);
    }

    public void GetScore(Action<ResourceResponse> OnResponse, Action OnGetAllScores)
    {
        StartCoroutine(GetScoreCoroutine(OnResponse, OnGetAllScores));
    }
    private IEnumerator GetScoreCoroutine(Action<ResourceResponse> OnResponse, Action OnGetAllScores)
    {
        List<ServerResponce> responces = new List<ServerResponce>();

        for (int i = 0; i < GameManager.Resources.Count; i++)
        {
            responces.Add(Server.Instance.GetResourceValue(GameManager.Resources[i]));
        }

        foreach (ServerResponce responce in responces)
        {
            yield return responce;
            UnfoldResourceAnswer(OnResponse, responce);
        }

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
    
    private static void UnfoldResourceAnswer(Action<ResourceResponse> OnResponse, ServerResponce responce, string resource = "")
    {
        if (responce.Error)
        {
            if (!string.IsNullOrEmpty(resource))
                Debug.LogErrorFormat("There is no {0}", resource);
            else
                Debug.LogErrorFormat("There is no resource");
        }
        else
        {
            Type type = typeof(ResourceResponse);
            var deserializedResponse = JsonConvert.DeserializeObject(responce.responce, type);

            if (deserializedResponse is ResourceResponse resourceResponce)
            {
                OnResponse?.Invoke(resourceResponce);
            }
        }
    }
}

public class ServerResponce
{
    public bool Error;
    public string responce;

    public ServerResponce(bool isError)
    {
        this.Error = isError;
        responce = null;
    }
    public ServerResponce(string responce)
    {
        this.responce = responce;
        Error = false;
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