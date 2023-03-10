using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviourPun
{
    [SerializeField]
    Sprite[] sprBoard;

    [SerializeField]
    Sprite[] playerTurn;

    [SerializeField]
    Image imgBoard;

    [SerializeField]
    List<Score> player1;

    [SerializeField]
    List<Score> player2;

    Queue<Sprite> boardQueue;

    public Dictionary<int, Dictionary<ScoreType, Score>> playerScore;
    public TMP_Text[] txtPlayers;

    private void Awake()
    {
        boardQueue = new Queue<Sprite>();
        boardQueue.Enqueue(sprBoard[1]);
        boardQueue.Enqueue(sprBoard[0]);

        playerScore = new Dictionary<int, Dictionary<ScoreType, Score>>();

        Dictionary<ScoreType, Score> temp = new Dictionary<ScoreType, Score>();
        foreach (Score item in player1)
            temp.Add(item.scoreType, item);

        playerScore.Add(1, temp);

        Dictionary<ScoreType, Score> temp2 = new Dictionary<ScoreType, Score>();
        foreach (Score item in player2)
            temp2.Add(item.scoreType, item);

        playerScore.Add(2, temp2);
    }


    public void ChangeBoard()=> photonView.RPC("RPCChangeBoard", RpcTarget.AllBuffered);
    [PunRPC]
    void RPCChangeBoard()
    {
        Sprite sprite = boardQueue.Dequeue();

        imgBoard.sprite = sprite;

        boardQueue.Enqueue(sprite);
    }

}


