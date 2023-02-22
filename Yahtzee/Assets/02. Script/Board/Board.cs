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

    public Dictionary<int, Dictionary<ScoreType, Score>> playerScore;

    [SerializeField]
    List<Score> player1;
    [SerializeField]
    List<Score> player2;

    private void Start()
    {
        playerScore = new Dictionary<int, Dictionary<ScoreType, Score>>();

        Dictionary<ScoreType, Score> temp = new Dictionary<ScoreType, Score>();
        foreach (Score item in player1)
            temp.Add(item.scoreType, item);

        playerScore.Add(1, temp);

        temp.Clear();

        foreach (Score item in player2)
            temp.Add(item.scoreType, item);

        playerScore.Add(2, temp);
    }

    //»ç¿ë¹ý
    //playerScore[1][ScoreType.Aces]
}


