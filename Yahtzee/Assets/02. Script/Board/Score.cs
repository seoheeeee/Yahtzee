using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ScoreType
{
    Aces,
    Deuces,
    Threes,
    Fours,
    Fives,
    Sixes,
    Subtotal,
    Bonus,
    Choice,
    FourCard,
    FullHouse,
    SmallStraight,
    LargeStraight,
    Yacht,
    Total
}

public class Score : MonoBehaviour
{
    [SerializeField]
    public ScoreType scoreType;
    [SerializeField]
    TMP_Text txtScore;
    //[SerializeField]
    //Button btnScore;


    void Start()
    {
        txtScore = transform.GetChild(0).GetComponent<TMP_Text>();
        //btnScore = GetComponent<Button>();
        //btnScore.enabled = false;
    }

    public void SetText()
    {

    }

}
