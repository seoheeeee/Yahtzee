using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DiceSpritesManager : MonoBehaviourPun
{
    
    public Image diceImg;

    public Sprite[] KeepDiceSpr;
    public int value;
    public int index;


    void Start()
    {
        diceImg.enabled = false;
    }
    public void SetSprite(int value, int index)
    {
        if (value <= 0)
            return;

        this.value = value;
        this.index = index;

        photonView.RPC("RPCSyncSprite", RpcTarget.AllBuffered, value, index);
    }

    public void RemoveSprite()
    {
        photonView.RPC("RPCRemoveSprite", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPCSyncSprite(int value, int index)
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
