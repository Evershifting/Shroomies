﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private ILoginBlock _loginBlock;
    [Space]
    [SerializeField] private GameObject _resourceUICountersParent;
    [SerializeField] private GameObject _resourceUIButtonsParent, _highScorePanel;
    [Header("Prefabs")]
    [SerializeField] private GameObject _resourceUICounter;
    [SerializeField] private GameObject _resourceUIButton;

    private Dictionary<string, IResourceCounter> _resourcesCounters = new Dictionary<string, IResourceCounter>();
    public delegate void OnScore(int value);
    public static event OnScore OnScoreEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _loginBlock = GetComponentInChildren<ILoginBlock>();
    }

    private void OnGetScore(ResourceResponse response)
    {
        if (response != null)
        {
            GameManager.Score += response.value;
        }
    }

    private void OnGetScoreFinal()
    {
        OnScoreEvent?.Invoke(GameManager.Score);
    }
    public void GenerateUI(string resource)
    {
        GameObject go = Instantiate(_resourceUICounter, _resourceUICountersParent.transform);
        _resourcesCounters.Add(resource, go.GetComponent<IResourceCounter>());
        _resourcesCounters[resource].Init(resource, 0);
        go.name = resource.First().ToString().ToUpper() + resource.Substring(1) + "Counter";


        go = Instantiate(_resourceUIButton, _resourceUIButtonsParent.transform);
        go.GetComponent<IResourceInputBlock>().Init(resource);
    }

    public void SetLoginBlock(string username)
    {
        _loginBlock.SetLoginBlock(username);
    }

    public void UpdateResource(ResourceResponse responce)
    {
        if (responce != null)
        {
            if (_resourcesCounters.ContainsKey(responce.resource))
                _resourcesCounters[responce.resource].ResourceAmount.text = responce.value.ToString();
        }
    }

    public void GetScore()
    {
        GameManager.Score = 0;
        APIs.Instance.GetScore(OnGetScore, OnGetScoreFinal);
    }

    public void SwitchHighScores()
    {
        _highScorePanel.SetActive(!_highScorePanel.activeSelf);
    }
}
