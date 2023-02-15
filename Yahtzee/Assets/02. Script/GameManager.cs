using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{

    enum State
    {

    }

    static GameManager instance;

    [SerializeField] Node startNode;
    [SerializeField] Dice[] dices;
    public List<PlayerManager> playerManagerList;

    Queue<PlayerManager> playerQueue;
    PlayerManager currentPlayer;

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




}
