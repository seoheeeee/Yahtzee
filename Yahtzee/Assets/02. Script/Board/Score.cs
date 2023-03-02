using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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

[RequireComponent(typeof(PhotonView))]

public class Score : MonoBehaviourPun
{
    [SerializeField]
    TMP_Text txtScore;
    [SerializeField]
    Button scoreBnt;

    Color green, black;
    public int score;

    public bool onClick;

    public ScoreType scoreType;

    private void Start()
    {
        txtScore = transform.GetChild(0).GetComponent<TMP_Text>();
        txtScore.text = "0";

        scoreBnt = transform.GetComponent<Button>();
        scoreBnt.enabled = true;
        scoreBnt.onClick.AddListener(GameManager.Instance.EndTurn);

        green = new Color(0, 128f / 255f, 0);
        black = Color.black;

    }

    

    public void SetScore(int score)
    {
        txtScore.text = $"{score}";
    }

    [PunRPC]
    void RPCSetScore(int score)
    {
        txtScore.text = $"{score}";
    }

    public void PVEndTurn(int score)
    {
        photonView.RPC("RPCSetScore", RpcTarget.AllBuffered, score);
    }

    public void OnClick()
    {
        score = int.Parse(txtScore.text);
        txtScore.color = Color.black;
        onClick = true;
        scoreBnt.enabled = false;
    }

    public void ActiveBtn()
    {
        if(!onClick)
        {
            scoreBnt.enabled = true;
            txtScore.color = green;
        }
    }

    public void DeactiveBtn()
    {
        scoreBnt.enabled = false;
        txtScore.color = black;
        PVEndTurn(int.Parse(txtScore.text));
    }
}
