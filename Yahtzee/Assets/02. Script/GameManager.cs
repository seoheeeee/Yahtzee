using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviourPun
{

    public Button startBnt;
    public Button stopBnt;

    enum State
    {

    }

    static GameManager instance;

    [SerializeField]
    List<Dice> diceList;

  

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

    public void DiceRoll()
    {
        startBnt.gameObject.SetActive(false);
        stopBnt.gameObject.SetActive(true);
        

        foreach (var item in diceList)
        {
            item.isRoll = true;
        }
    }

    public void DiceStop()
    {
        stopBnt.gameObject.SetActive(false);

        foreach (var item in diceList)
        {
            item.isEnd = true;
        }
    }
}
