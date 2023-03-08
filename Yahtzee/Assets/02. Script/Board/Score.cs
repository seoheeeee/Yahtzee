using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public TMP_Text txtScore;
    public Button scoreBtn;

    Color green, gray;
    public int score;

    public bool onClick;

    public ScoreType scoreType;

    private void Awake()
    {
        txtScore = transform.GetChild(0).GetComponent<TMP_Text>();
        
        if (scoreType != ScoreType.Subtotal)
            txtScore.text = "";
        else
            txtScore.text = "0 / 63";

        scoreBtn = transform.GetComponent<Button>();
        scoreBtn.enabled = true;

        gray = new Color(30f / 255f, 30f / 255f, 30f / 255f, 0.5f);
        green = new Color(0, 128f / 255f, 0, 0.7f);
        //black = Color.black;
    }
    private void Start()
    {
        scoreBtn.onClick.AddListener(OnClick);
        scoreBtn.onClick.AddListener(GameManager.Instance.EndTurn);
        DeactiveBtn();
    }
    private void Update()
    {
        if (scoreType == ScoreType.Bonus) scoreBtn.enabled = false;
        if (scoreType == ScoreType.Subtotal) scoreBtn.enabled = false;
        if (scoreType == ScoreType.Total) scoreBtn.enabled = false;
    }

    [PunRPC]
    void RPCSetScore(int score)
    {
        if (ScoreType.Subtotal == scoreType)
        {
            txtScore.text = $"{score} / 63";
            return;
        }
        else if (ScoreType.Bonus == scoreType)
        {
            if (score != 0)
                txtScore.text = $"+{score}";
            return;
        }
        if (!onClick)
        {
            txtScore.color = gray;
            txtScore.text = $"{score}";
        }
    }

    public void SetScore(int score)
    {
        photonView.RPC("RPCSetScore", RpcTarget.AllBuffered, score);
    }

    public void OnClick()
    {
        score = int.Parse(txtScore.text);
        SetScore(score);
        photonView.RPC("OnClickButton", RpcTarget.AllBuffered);
        scoreBtn.enabled = false;
    }

    [PunRPC]
    void OnClickButton() 
    {
        txtScore.color = Color.black;
        onClick = true;
    }
    public void ActiveBtn()
    {
        if (!onClick)
        {
            scoreBtn.enabled = true;
            if (scoreType != ScoreType.Subtotal)
                txtScore.color = green;
        }
    }

    public void DeactiveBtn()
    {
        scoreBtn.enabled = false;
        if (scoreType != ScoreType.Subtotal && txtScore.text != "")
        {
            txtScore.color = Color.black;
            SetScore(int.Parse(txtScore.text));
        }
    }
}
