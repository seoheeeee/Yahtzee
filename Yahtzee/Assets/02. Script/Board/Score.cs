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

    Color green, black;
    public int score;

    public bool onClick;

    public ScoreType scoreType;

    private void Awake()
    {
        txtScore = transform.GetChild(0).GetComponent<TMP_Text>();
        txtScore.text = "0";

        scoreBtn = transform.GetComponent<Button>();
        scoreBtn.enabled = true;


        green = new Color(0, 128f / 255f, 0);
        black = Color.black;
    }
    private void Start()
    {
        scoreBtn.onClick.AddListener(OnClick);
        scoreBtn.onClick.AddListener(GameManager.Instance.EndTurn);
    }
    private void Update()
    {
        if (scoreType == ScoreType.Bonus) scoreBtn.enabled = false;
        if (scoreType == ScoreType.Subtotal) scoreBtn.enabled = false;
        if (scoreType == ScoreType.Total) scoreBtn.enabled = false;
    }

    public void SetScore(int score)
    {
        if (!onClick)
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
        PVEndTurn(score);
        txtScore.color = Color.black;
        photonView.RPC("OnClickButton", RpcTarget.AllBuffered);
        scoreBtn.enabled = false;
    }

    [PunRPC]
    void OnClickButton() => onClick = true;
    public void ActiveBtn()
    {
        if (!onClick)
        {
            scoreBtn.enabled = true;
            txtScore.color = green;
        }
    }

    public void DeactiveBtn()
    {
        scoreBtn.enabled = false;
        txtScore.color = black;
        PVEndTurn(int.Parse(txtScore.text));
    }
}
