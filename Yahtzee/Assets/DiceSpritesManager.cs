using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSpritesManager : MonoBehaviour
{
    
    public Image diceImg;

    public Sprite[] KeepDiceSpr;
    public Dice dice;

    void Start()
    {
        diceImg.enabled = false;
    }

    
    void Update()
    {
        
    }

    public void SetSprite(Dice dice)
    {
        if (dice.value <= 0)
            return;
        this.dice = dice;       
        diceImg.sprite = KeepDiceSpr[dice.value -1];
        diceImg.enabled = true;
    }

    public void RemoveSprite()
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
