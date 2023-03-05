using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Threading.Tasks.Sources;

public class DiceSpritesManager : MonoBehaviourPun
{
    
    public Image diceImg;
    public Button button;
    public Sprite[] KeepDiceSpr;
    public int value;
    public int index;


    void Start()
    {
        button = GetComponent<Button>();
        diceImg.enabled = false;
    }
    public void SetSprite(int index, int value)
    {
        if (value <= 0)
            return;

        this.value = value;
        this.index = index;

        photonView.RPC("RPCSyncSprite", RpcTarget.AllBuffered, index, value);
    }

    public void RemoveSprite()
    {
        photonView.RPC("RPCRemoveSprite", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPCSyncSprite(int index, int value)
    {
        diceImg.sprite = KeepDiceSpr[value - 1];
        this.value = value;
        this.index = index;
        diceImg.enabled = true;
    }

    [PunRPC]
    void RPCRemoveSprite()
    {
        if (diceImg.enabled)
        {
            GameManager.Instance.diceList[index].gameObject.SetActive(true);
            diceImg.enabled = false;
            diceImg.sprite = null;
            GameManager.Instance.keepDiceCount--;
        }
    }
}
