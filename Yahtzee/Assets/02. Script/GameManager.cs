using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    PlayGame,
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
        turn = 1;
        txtChance.text = "1 / 13";
        txtChance.text = chance.ToString();

        PlayerManager[] tempPlayer = FindObjectsOfType<PlayerManager>();

        foreach (PlayerManager item in tempPlayer)
        {
            if (item.num == 1)
            {
                curPlayer = item;
                curPlayer.isTurn = true;
                board.txtPlayers[item.num - 1].text = item.gameObject.name;
            }
            else
            {
                restPlayer = item;
                board.txtPlayers[item.num - 1].text = item.gameObject.name;
            }
        }
        startBnt.gameObject.SetActive(false);
        stopBnt.gameObject.SetActive(false);

        diceDot = new Dictionary<int, int>();

        for (int i = 1; i <= 6; i++)
        {
            diceDot.Add(i, 0);
        }

        Turn(false);
    }

    private void Update()
    {
        txtChance.text = chance.ToString();

        switch (state)
        {
            case State.PlayGame:
                //if (curPlayer.turn == 0 && restPlayer.turn == 0)

                if (keepDiceCount < 5)
                {
                    isPewivew = false;
                    foreach (var item in board.playerScore[curPlayer.num])
                    {
                        if (!item.Value.onClick)
                        {
                            item.Value.scoreBtn.enabled = false;
                            item.Value.txtScore.color = Color.black;

                            if (item.Key == ScoreType.Total) continue;
                            else if (item.Key == ScoreType.Subtotal) continue;
                            else if (item.Key == ScoreType.Bonus) continue;

                            item.Value.SetScore(0);
                            item.Value.score = 0;

                        }
                    }
                }
                if (chance == 3)
                    startBnt.gameObject.SetActive(false);
                break;
        }
    }


    public void StartRoll()
    {

        if ( state != State.PlayGame ||
            keepDiceCount == 5)
            return;

        foreach (var item in diceList)
        {
            if (item.gameObject.activeSelf)
            {
                item.isRoll = true;
            }
        }
        startBnt.gameObject.SetActive(false);
        stopBnt.gameObject.SetActive(true);
        chance++;
        state = State.EndGame;
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
    bool isPewivew;
    public void PreviewScore(int playerNum)
    {

        if (!isPewivew && keepDiceCount > 4)
        {
            for (int i = 1; i < 7; i++)
                diceDot[i] = 0;

            int temp = 0;
            bool isTrue = false;

            foreach (Dice item in diceList)
            {
                if (item.value == 0) return;
                diceDot[item.value] += 1;
            }

            foreach (var item in board.playerScore[playerNum])
            {
                item.Value.ActiveBtn();

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
                    case ScoreType.Subtotal:
                        break;
                    case ScoreType.Bonus:
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
                            item.Value.SetScore(temp);
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
                            item.Value.SetScore(15);
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
                            item.Value.SetScore(30);
                        else
                            item.Value.SetScore(0);
                        break;
                    case ScoreType.Yacht:
                        foreach (var dot in diceDot)
                        {
                            if (dot.Value == 5)
                            {
                                item.Value.SetScore(50);
                                break;
                            }
                        }
                        if
                            (temp == 0) item.Value.SetScore(temp);
                        break;
                    case ScoreType.Total:
                        break;
                }
                temp = 0;
                isTrue = false;
            }
            isPewivew = true;
        }
    }

    public void EndTurn()
    {
        int totalScore = 0;
        chance = 0;

        foreach (var item in board.playerScore[curPlayer.num])
        {
            if (!item.Value.onClick) item.Value.SetScore(0);

            item.Value.DeactiveBtn();
            totalScore += item.Value.score;
            switch (item.Key)
            {
                case ScoreType.Subtotal:
                    item.Value.PVEndTurn(totalScore);
                    break;
                case ScoreType.Bonus:
                    if (totalScore > 62)
                    {
                        item.Value.PVEndTurn(35);
                        totalScore += 35;
                    }
                    break;
                case ScoreType.Total:
                    item.Value.PVEndTurn(totalScore);
                    break;
            }
        }

        if(curPlayer.num == 2)
        {
            turn++;

            photonView.RPC("RPCTurnProgress", RpcTarget.AllBuffered, turn);
        }

        photonView.RPC("ChangePlayer", RpcTarget.AllBuffered);

        

        board.ChangeBoard();
    }

    [PunRPC]
    void ChangePlayer()
    {
        PlayerManager temp = curPlayer;
        curPlayer = restPlayer;
        restPlayer = temp;

        curPlayer.isTurn = true;
        restPlayer.isTurn = false;
        isPewivew = false;
        keepDiceCount = 0;

        foreach (Dice item in diceList)
            item.GameReset();

        foreach (DiceSpritesManager item in spriteManager)
        {
            item.index = 0;
            item.value = 0;
            item.diceImg.enabled = false;
            item.diceImg.sprite = null;
        }
    }

    public void Turn(bool isTurn, int num = 0)
    {
        if (isTurn)
        {
            if (state == State.PlayGame)
                startBnt.gameObject.SetActive(true);
            else
                startBnt.gameObject.SetActive(false);

            if (keepDiceCount == 5)
                board.ActiveButtons(num, true);
            else
                board.ActiveButtons(num, false);
        }
        else
        {
            //startBnt.gameObject.SetActive(false);
            board.ActiveButtons(1, false);
            board.ActiveButtons(2, false);

        }
    }

    [PunRPC]
    void RPCTurnProgress(int turn)
    {
        txtTurn.text = $"{turn} / 13";
    }
}
