using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DiceSpritesManager : MonoBehaviourPun
{
    
    public Image diceImg;

    public Sprite[] KeepDiceSpr;
    public Dice dice;

    void Start()
    {
        diceImg.enabled = false;
    }
    public void SetSprite(Dice dice)
    {
        if (dice.value <= 0)
            return;
        this.dice = dice;
        photonView.RPC("RPCSyncSprite", RpcTarget.AllBuffered, dice.value - 1);
    }

    public void RemoveSprite()
    {
        photonView.RPC("RPCRemoveSprite", RpcTarget.AllBuffered);

    }

    [PunRPC]
    void RPCSyncSprite(int index)
    {
        diceImg.sprite = KeepDiceSpr[index];
        diceImg.enabled = true;
    }

    [PunRPC]
    void RPCRemoveSprite()
    {
        if (diceImg.enabled)
        {
            diceImg.enabled = false;
            dice.gameObject.SetActive(true);
            diceImg.sprite = null;
            GameManager.Instance.keepDiceCount--;
        }
    }
}
