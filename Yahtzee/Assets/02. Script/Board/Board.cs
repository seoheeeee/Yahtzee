using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField]
    Sprite board;
    [SerializeField]
    Sprite[] playerTurn;

    [SerializeField]
    List<Score> player1ScoreList;
    [SerializeField]
    List<Score> player2ScoreList;

    public Dictionary<int,Dictionary<ScoreType, Score>> playerScore;

    private void Start()
    {
        playerScore = new Dictionary<int,Dictionary<ScoreType, Score>>();

        Dictionary<ScoreType, Score> score = new Dictionary<ScoreType, Score>();

        foreach (Score item in player1ScoreList) 
            score.Add(item.scoreType, item);
        
        playerScore.Add(1, score);

        score.Clear();

        foreach (Score item in player2ScoreList)
            score.Add(item.scoreType, item);

        playerScore.Add(2, score);

        playerScore[1][ScoreType.Deuces].SetText();
    }
}
