using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreRecord : MonoBehaviour, IHighScoreRecord
{
    [SerializeField] private TextMeshProUGUI _name, _score;
    
    public void Init(string name, int score)
    {
        _name.text = name;
        _score.text = score.ToString();
    }
}
