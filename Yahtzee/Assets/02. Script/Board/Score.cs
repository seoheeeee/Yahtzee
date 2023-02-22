using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtScore;

    int score;

    private void Start()
    {
        txtScore = transform.GetChild(0).GetComponent<TMP_Text>();
        txtScore.text = "0";
    }

    public void SetScore(int score)
    {
        txtScore.text = $"{score}";
        this.score = score;
    }
    public int GetScore()
    {
        return score;
    }
}
