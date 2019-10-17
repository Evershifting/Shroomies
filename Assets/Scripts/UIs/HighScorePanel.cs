using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScorePanel : MonoBehaviour
{
    [SerializeField] private int _amountOfRecordings = 5;
    [SerializeField] private HighScores _scoresSource;
    [SerializeField] private GameObject _scoreRecordingPrefab, _scoreRecordingsParent;

    private void OnEnable ()
    {
        UIManager.OnScoreEvent += new UIManager.OnScore(OnGotScore);
        CleanRecords();
        GenerateRecords();
    }

    private void CleanRecords()
    {
        foreach (Transform item in _scoreRecordingsParent.transform)
        {
            Destroy(item.gameObject);
        }
    }

    private void GenerateRecords()
    {
        ScoreRecord sr;
        for (int i = 0; i < _amountOfRecordings; i++)
        {
            GameObject go = Instantiate(_scoreRecordingPrefab, _scoreRecordingsParent.transform);
            sr = _scoresSource.GetScores();
            go.GetComponent<IHighScoreRecord>().Init(sr.Name, sr.Score);
        }
        UIManager.Instance.GetScore();
    }

    private void OnDisable()
    {
        UIManager.OnScoreEvent -= OnGotScore;
    }

    private void OnGotScore(int value)
    {
        GameObject go = Instantiate(_scoreRecordingPrefab, _scoreRecordingsParent.transform);
        go.transform.SetAsFirstSibling();
        go.GetComponent<IHighScoreRecord>().Init(GameManager.username, value);
    }
}
