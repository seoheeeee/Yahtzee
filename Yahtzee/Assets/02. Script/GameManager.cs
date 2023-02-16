using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPun
{
    [SerializeField]
    Button startBnt;
    [SerializeField]
    Button stopBnt;
   

   

    public List<Dice> DiceList;
    [SerializeField]
    List<DiceSpritesManager> spriteManager;

    static GameManager instance;

   

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

    }

    private void Update()
    {
        
    }

    public void StartRoll()
    {
        startBnt.gameObject.SetActive(false);
        stopBnt.gameObject.SetActive(true);

        foreach (var item in DiceList)
        {
            item.isRoll = true;
        }
    }

    public void StopRoll()
    {
        stopBnt.gameObject.SetActive(false);

        foreach (var item in DiceList)
        {
            item.isEnd = true;
        }
    }

    public void SelectDice(Dice dice)
    {
        foreach (var item in spriteManager)
        {
            if(item.diceImg.sprite == null)
            {
                item.SetSprite(dice);
                break;
            }
        }
    }
}
