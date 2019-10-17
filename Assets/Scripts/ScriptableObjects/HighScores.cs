using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class HighScores : ScriptableObject
{
    [SerializeField] private List<string> _names = new List<string>() { "Ennie", "Minnie", "Mo" };
    [SerializeField] private List<int> _scores = new List<int>() { 420, 555, 19762, 0 };

    public ScoreRecord GetScores()
    {
        return new ScoreRecord(
            _names.Count > 0 ? _names[Random.Range(0, _names.Count)] : "RandomUsername",
            _scores.Count > 0 ? _scores[Random.Range(0, _scores.Count)] : Random.Range(0, 1000));
    }
}

public struct ScoreRecord
{
    public string Name;
    public int Score;

    public ScoreRecord(string name, int score)
    {
        Name = name;
        Score = score;
    }
}
