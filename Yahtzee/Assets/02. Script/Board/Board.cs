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

    public Dictionary<int, List<Score>> playerScore;

    [SerializeField]
    List<Score> player1;
    [SerializeField]
    List<Score> player2;

    private void Start()
    {
        playerScore = new Dictionary<int, List<Score>>();

        playerScore.Add(1, player1);
        playerScore.Add(2, player2);

        Debug.Log(playerScore);
    }


}
