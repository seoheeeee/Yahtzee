using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ScoreType 
{
    Aces,
    Deuces,
    Threes,
    Fours,
    Fives,
    Sixes,
    Subtotal,
    Bonus,
    Choice,
    FourKind,
    FullHouse,
    S_Straight,
    L_Straight,
    Yacht,
    Total
}

public class Score : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtScore;

    int score;

    public bool onClick;

    public ScoreType scoreType;

    private void Start()
    {
        txtScore = transform.GetChild(0).GetComponent<TMP_Text>();
        txtScore.text = "0";
    }

    public void SetScore(int score, Color color)
    {
        txtScore.text = $"{score}";
        txtScore.color = color;
    }
    public int GetScore()
    {
        return score;
    }

    
}
