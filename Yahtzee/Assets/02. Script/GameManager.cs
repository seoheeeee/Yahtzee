using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Phase1,
    Phase2,
    Phase3,
    End
}

public class GameManager : MonoBehaviourPun
{
    public State state;
    static GameManager instance;

    [SerializeField]
    Button startBnt;
    [SerializeField]
    Button stopBnt;
    [SerializeField]
    Board board;
    [SerializeField]
    List<DiceSpritesManager> spriteManager;

    [SerializeField]
    PlayerManager curPlayer;
    PlayerManager restPlayer;

    public List<Dice> diceList;
    public int selectDiceCount;
    public int keepDiceCount;

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
        PlayerManager[] tempPlayer = FindObjectsOfType<PlayerManager>();

        foreach (PlayerManager item in tempPlayer)
        {
            if (item.num == 1)
                curPlayer = item;
            else
                restPlayer = item;                    
        }

        startBnt.gameObject.SetActive(true);
        stopBnt.gameObject.SetActive(false);

        diceDot = new Dictionary<int, int>();

        for (int i = 1; i <= 6; i++)
        {
            diceDot.Add(i, 0);
        }
        //board.playerScore[1][ScoreType.Aces].onClick;
    }

    private void Update()
    {
        
    }

    public void StartRoll()
    {
        startBnt.gameObject.SetActive(false);
        stopBnt.gameObject.SetActive(true);

        foreach (var item in diceList)
        {
            if (item.gameObject.activeSelf)
                item.isRoll = true;
        }
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

    public void SelectDice(Dice dice)
    {
        foreach (var item in spriteManager)
        {
            if (item.diceImg.sprite == null)
            {
                item.SetSprite(dice);
                keepDiceCount++;
                break;
            }
        }
    }

    void PreviewScore(int playerNum)
    {
        int temp = 0;
        bool isTrue = false;
        
        foreach (Dice item in diceList)
        {
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
                        temp += dot.Value;
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
                            if(diceDot[temp] == 3)
                            {
                                if(dot.Value == 2)
                                {
                                    int temp2 = (temp * diceDot[temp]) +
                                                (dot.Value * dot.Key);
                                    item.Value.SetScore(temp2);
                                    fullHouse = true;
                                    break;
                                }
                            }
                            else if(diceDot[temp] == 2)
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
                    if(!fullHouse) 
                        item.Value.SetScore(0);
                    break;
                case ScoreType.S_Straight:
                    for (int i = 1; i <= 6; i++)
                    {
                        if (diceDot[i] != 0)
                            temp++;
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
                            temp++;
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
                        if(dot.Value == 5)
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
    }

    public void EndTurn()
    {
        int totalScore = 0;

        foreach (var item in board.playerScore[curPlayer.num])
        {
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
        photonView.RPC("ChangePlayer", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ChangePlayer()
    {
        PlayerManager temp = curPlayer;
        curPlayer = restPlayer;
        restPlayer = temp;
    }

    [PunRPC]
    void CurrentPlayer()
    {
        
    }
}
