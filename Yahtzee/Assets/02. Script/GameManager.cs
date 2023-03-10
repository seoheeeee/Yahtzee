using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    PlayGame,
    DiceRoll,
    EndGame
}


public class GameManager : MonoBehaviourPun
{
    public State state;

    static GameManager instance;

    [SerializeField]
    TMP_Text txtChance;

    [SerializeField]
    Button stopBnt;
    [SerializeField]
    Board board;

    [SerializeField]
    PlayerManager curPlayer;
    [SerializeField]
    PlayerManager restPlayer;
    [SerializeField]
    TMP_Text txtResult;

    public TMP_Text txtTurn;
    public Button startBnt;
    public List<DiceSpritesManager> spriteManager;
    public List<Dice> diceList;
    //public int selectDiceCount;
    public int keepDiceCount;
    public int chance;
    int turn;

    Dictionary<int, int> diceDot;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
        private set => instance = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
                Destroy(gameObject);
        }
    }

    void Start()
    {
        photonView.RPC("RPCChance", RpcTarget.AllBuffered, chance);
        turn = 1;
        txtTurn.text = "1 / 12";
        chance = 3;

        PlayerManager[] tempPlayer = FindObjectsOfType<PlayerManager>();

        foreach (PlayerManager item in tempPlayer)
        {
            if (item.num == 1)
            {
                curPlayer = item;
                curPlayer.isTurn = true;
                if (item.gameObject.name != "")
                    board.txtPlayers[item.num - 1].text = item.gameObject.name;
                else
                    board.txtPlayers[item.num - 1].text = "Player1";
            }
            else
            {
                restPlayer = item;
                if (item.gameObject.name != "")
                    board.txtPlayers[item.num - 1].text = item.gameObject.name;
                else
                    board.txtPlayers[item.num - 1].text = "Player2";
            }
        }
        startBnt.gameObject.SetActive(false);
        stopBnt.gameObject.SetActive(false);

        diceDot = new Dictionary<int, int>();

        for (int i = 1; i <= 6; i++)
        {
            diceDot.Add(i, 0);
        }

        photonView.RPC("RPCChance", RpcTarget.AllBuffered, chance);
        txtResult.gameObject.SetActive(false);
    }

    private void Update()
    {
        //txtChance.text = chance.ToString();
        if (photonView.IsMine)
        {
            switch (state)
            {
                case State.PlayGame:
                    if (curPlayer.isTurn)
                    {
                        if (chance == 0)
                            startBnt.gameObject.SetActive(false);
                        else
                            startBnt.gameObject.SetActive(true);

                        foreach (var item in spriteManager)
                            item.button.enabled = true;
                    }
                    break;

                case State.DiceRoll:
                    foreach (var item in spriteManager)
                        item.button.enabled = false;

                    foreach (var item in board.playerScore[curPlayer.num])
                    {
                        if (!item.Value.onClick)
                        {
                            item.Value.scoreBtn.enabled = false;
                            if (item.Key != ScoreType.Subtotal)
                                item.Value.txtScore.color = Color.black;

                            if (item.Key == ScoreType.Total) continue;
                            else if (item.Key == ScoreType.Subtotal) continue;
                            else if (item.Key == ScoreType.Bonus) continue;
                        }
                    }

                    foreach (Dice item in diceList)
                        if (!item.isStop) return;

                    PreviewScore(curPlayer.num);

                    state = State.PlayGame;
                    break;
            }
        }
        else
        {
            startBnt.gameObject.SetActive(false);
            foreach (var item in spriteManager)
                item.button.enabled = false;
        }

    }
    [PunRPC]
    void ChangeState(int state)
    {
        this.state = (State)state;
    }

    public void StartRoll()
    {

        if (state != State.PlayGame ||
            keepDiceCount == 5)
            return;

        foreach (var item in diceList)
        {
            if (item.gameObject.activeSelf)
            {
                item.isStop = false;
                item.isRoll = true;
            }
        }
        startBnt.gameObject.SetActive(false);
        stopBnt.gameObject.SetActive(true);
        chance--;
        photonView.RPC("RPCChance", RpcTarget.AllBuffered, chance);
        photonView.RPC("ResetBoard", RpcTarget.AllBuffered);
        state = State.DiceRoll;
    }


    [PunRPC]
    void ResetBoard()
    {
        foreach (var item in board.playerScore[curPlayer.num])
        {
            item.Value.txtScore.color = Color.black;
            if (item.Key == ScoreType.Bonus) continue;
            else if (item.Key == ScoreType.Subtotal) continue;
            else if (item.Key == ScoreType.Total) continue;
            else if (!item.Value.onClick)
            {
                item.Value.txtScore.text = "";
                item.Value.score = 0;
            }
        }
    }

    [PunRPC]
    void RPCChance(int chance)
    {
        txtChance.text = chance.ToString();
    }
    public void StopRoll()
    {
        stopBnt.gameObject.SetActive(false);

        foreach (var item in diceList)
        {
            if (item.gameObject.activeSelf)
                item.isEnd = true;
        }
    }

    public void SelectDice(int index, int value)
    {
        photonView.RPC("RPCSelectDice", RpcTarget.AllBuffered, index, value);
    }

    [PunRPC]
    void RPCSelectDice(int index, int value)
    {
        foreach (var item in spriteManager)
        {
            if (item.diceImg.sprite == null)
            {
                item.SetSprite(index, value);
                diceList[index].gameObject.SetActive(false);
                keepDiceCount++;
                break;
            }
        }
    }
    public void PreviewScore(int playerNum)
    {
        for (int i = 1; i < 7; i++)
            diceDot[i] = 0;

        int temp = 0;
        bool isTrue = false;
        ScoreType maxScore = ScoreType.Aces;
        int max = 0;

        foreach (Dice item in diceList)
        {
            //if (item.value == 0) continue;
            if (item.gameObject.activeSelf)
                diceDot[item.value] += 1;
        }

        foreach (DiceSpritesManager item in spriteManager)
        {
            if (item.diceImg.enabled)
                diceDot[item.value] += 1;
        }

        foreach (var item in board.playerScore[playerNum])
        {


            switch (item.Key)
            {
                case ScoreType.Aces:
                    item.Value.SetScore(diceDot[1]);
                    break;
                case ScoreType.Deuces:
                    item.Value.SetScore(diceDot[2] * 2);
                    break;
                case ScoreType.Threes:
                    item.Value.SetScore(diceDot[3] * 3);
                    break;
                case ScoreType.Fours:
                    item.Value.SetScore(diceDot[4] * 4);
                    break;
                case ScoreType.Fives:
                    item.Value.SetScore(diceDot[5] * 5);
                    break;
                case ScoreType.Sixes:
                    item.Value.SetScore(diceDot[6] * 6);
                    break;
                case ScoreType.Choice:
                    foreach (var dot in diceDot)
                    {
                        temp += dot.Key * dot.Value;
                    }
                    item.Value.SetScore(temp);
                    break;
                case ScoreType.FourKind:
                    foreach (var dot in diceDot)
                    {
                        temp += dot.Key * dot.Value;
                        if (dot.Value == 4)
                        {
                            isTrue = true;
                        }
                    }
                    if (isTrue)
                    {
                        if (max < temp)
                        {
                            maxScore = ScoreType.FourKind;
                            max = temp;
                        }
                        item.Value.SetScore(temp);
                    }
                    else
                        item.Value.SetScore(0);

                    break;
                case ScoreType.FullHouse:
                    bool fullHouse = false;
                    foreach (var dot in diceDot)
                    {
                        if (!isTrue)
                        {
                            if (dot.Value == 2 || dot.Value == 3)
                            {
                                temp = dot.Key;
                                isTrue = true;
                            }
                        }
                        else
                        {
                            if (diceDot[temp] == 3)
                            {
                                if (dot.Value == 2)
                                {
                                    int temp2 = (temp * diceDot[temp]) +
                                                (dot.Value * dot.Key);
                                    item.Value.SetScore(temp2);
                                    fullHouse = true;
                                    if (max < temp2)
                                    {
                                        maxScore = ScoreType.FullHouse;
                                        max = temp2;
                                    }
                                    break;
                                }
                            }
                            else if (diceDot[temp] == 2)
                            {
                                if (dot.Value == 3)
                                {
                                    int temp2 = (temp * diceDot[temp]) +
                                                (dot.Value * dot.Key);
                                    item.Value.SetScore(temp2);
                                    fullHouse = true;
                                    if (max < temp2)
                                    {
                                        maxScore = ScoreType.FullHouse;
                                        max = temp2;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if (!fullHouse)
                        item.Value.SetScore(0);
                    break;
                case ScoreType.S_Straight:
                    for (int i = 1; i <= 6; i++)
                    {
                        if (diceDot[i] != 0)
                        {
                            temp++;
                            if (temp == 4) break;
                        }
                        else
                            temp = 0;
                    }

                    if (temp == 4)
                    {
                        item.Value.SetScore(15);
                        if (max < 15)
                        {
                            maxScore = ScoreType.S_Straight;
                            max = 15;
                        }
                    }
                    else
                        item.Value.SetScore(0);
                    break;
                case ScoreType.L_Straight:
                    for (int i = 1; i <= 6; i++)
                    {
                        if (diceDot[i] != 0)
                        {
                            temp++;
                            if (temp == 5) break;
                        }
                        else
                            temp = 0;
                    }
                    if (temp == 5)
                    {
                        item.Value.SetScore(30);

                        if (max < 30)
                        {
                            maxScore = ScoreType.L_Straight;
                            max = 30;
                        }

                    }
                    else
                        item.Value.SetScore(0);
                    break;
                case ScoreType.Yacht:
                    foreach (var dot in diceDot)
                    {
                        if (dot.Value == 5)
                        {
                            maxScore = ScoreType.Yacht;
                            max = 50;

                            item.Value.SetScore(50);
                            temp = 50;
                            break;
                        }
                    }
                    if (temp == 0) item.Value.SetScore(temp);
                    break;
                case ScoreType.Total:
                    break;
            }
            temp = 0;
            isTrue = false;
            item.Value.ActiveBtn();
        }

        if (maxScore == ScoreType.S_Straight ||
            maxScore == ScoreType.L_Straight ||
            maxScore == ScoreType.FourKind ||
            maxScore == ScoreType.Yacht ||
            maxScore == ScoreType.FullHouse)
            photonView.RPC("Result", RpcTarget.AllBuffered, maxScore.ToString());
    }
    [PunRPC]
    void Result(string result)
    {
        txtResult.text = result.Replace("_", ". ");
        StartCoroutine(IResult());
    }
    IEnumerator IResult()
    {
        txtResult.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        txtResult.gameObject.SetActive(false);
    }
    public void EndTurn()
    {
        chance = 0;
        photonView.TransferOwnership(PhotonNetwork.PlayerList[restPlayer.num - 1]);

        if (curPlayer.num == 2)
        {
            turn++;
            
            photonView.RPC("RPCTurnProgress", RpcTarget.AllBuffered, turn); 
        }

        int totalScore = 0;

        foreach (var item in board.playerScore[curPlayer.num])
        {
            if (!item.Value.onClick) item.Value.SetScore(0);

            item.Value.DeactiveBtn();
            totalScore += item.Value.score;
            switch (item.Key)
            {
                case ScoreType.Subtotal:
                    item.Value.SetScore(totalScore);
                    break;
                case ScoreType.Bonus:
                    if (totalScore > 62)
                    {
                        item.Value.SetScore(35);
                        totalScore += 35;
                    }
                    break;
                case ScoreType.Total:
                    item.Value.SetScore(totalScore);
                    break;
            }
        }


        photonView.RPC("ResetBoard", RpcTarget.AllBuffered);
        photonView.RPC("ChangePlayer", RpcTarget.AllBuffered);
        
        photonView.RPC("RPCChance", RpcTarget.AllBuffered, chance);
        board.ChangeBoard();
    }

    [PunRPC]
    void ChangePlayer()
    {

        PlayerManager temp = curPlayer;
        curPlayer = restPlayer;
        restPlayer = temp;
        chance = 3;
        keepDiceCount = 0;
        curPlayer.isTurn = true;
        restPlayer.isTurn = false;

        foreach (Dice item in diceList)
        {
            item.GameReset(curPlayer.num - 1);
            item.isStop = false;
        }

        foreach (DiceSpritesManager item in spriteManager)
        {
            item.index = 0;
            item.value = 0;
            item.diceImg.enabled = false;
            item.diceImg.sprite = null;
        }
    }

    [PunRPC]
    void RPCTurnProgress(int turn)
    {
        if(turn == 13)
        {
            state = State.EndGame;
            return;
        }
        txtTurn.text = $"{turn} / 12";
    }
}
