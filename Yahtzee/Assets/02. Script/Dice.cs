using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Photon.Pun;

public class Dice : MonoBehaviourPun
{
    [SerializeField]
    Transform dice;
    [SerializeField]
    DiceSide[] diceSides;

    System.Random rand;
    Vector3[] randRotation = new Vector3[3];
    Quaternion startRotation;
    Quaternion endRotation;
    
    float speed;
    
    public bool isRoll;
    public bool isEnd;
    public bool isStop;
    public bool enabelBtn;
    public int value;
    public int index;

    [SerializeField]
    Material dotActice;
    [SerializeField]
    Material dotMaterial;

    private void Start()
    {
        rand = new System.Random();

        int index = rand.Next(randRotation.Length);

        randRotation[0] = new Vector3(90, 0, 0);
        randRotation[1] = new Vector3(0, 90, 0);
        randRotation[2] = new Vector3(0, 0, 90);
        startRotation = dice.rotation;
        endRotation = Quaternion.Euler(randRotation[index]);
    }


    private void Update()
    {
        if (isRoll)
        {
            isStop = false;
            foreach (DiceSide item in diceSides)
                item.meshRenderer.material = dotMaterial;

            speed += Time.deltaTime * 8;
            dice.rotation = Quaternion.Lerp(startRotation,
                 endRotation, speed);

            if (1 < speed)
            {
                int index = rand.Next(randRotation.Length);
                startRotation = dice.rotation;
                endRotation = startRotation * Quaternion.Euler(randRotation[index]);
                speed = 0;

                if (isEnd)
                {
                    isRoll = false;
                    isEnd = false;

                    StartCoroutine(EndDiceRoll(5));
                }
            }
        }
    }

    IEnumerator EndDiceRoll(int count)
    {
        for (int i = count; i > 0; i--)
        {
            while (true)
            {
                speed += Time.deltaTime * (8 - count);
                dice.rotation = Quaternion.Lerp(startRotation,
                     endRotation, speed);

                if (2f < speed)
                {
                    int index = rand.Next(randRotation.Length);
                    startRotation = dice.rotation;
                    endRotation = startRotation * Quaternion.Euler(randRotation[index]);
                    speed = 0;
                    break;
                }

                yield return null;

            }
        }

        foreach (DiceSide item in diceSides)
        {
            if (item.isActive)
            {
                value = item.sideValue;
                item.meshRenderer.material = dotActice;
            }
        }

        isStop = true;
    }

    public void SelectDice2()
    {
        if (value == 0)
            return;
        GameManager.Instance.SelectDice(index, value);
    }

    //[PunRPC]
    //void RPCAtiveDice(bool activate)
    //{
    //    gameObject.SetActive(activate);
    //}

    //public void ActiveDice(bool activate)
    //{
    //    photonView.RPC("RPCAtiveDice", RpcTarget.AllBuffered, activate);
    //}

    #region Old Dice
    //[SerializeField] Rigidbody rb;
    //[SerializeField] DiceSide[] diceSides;

    //[SerializeField] bool hasLanded;
    //[SerializeField] bool thrown;
    //Vector3 initPostion;
    //public int diceValue;

    //private void Start()
    //{
    //    initPostion = transform.position;

    //    rb.useGravity = false;

    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        RollDice();
    //    }
    //    if(rb.IsSleeping() && !hasLanded && thrown)
    //    {
    //        hasLanded = true;
    //        rb.useGravity = false;
    //    }
    //    else if(rb.IsSleeping() && !hasLanded && thrown)
    //    {
    //        RollAgain();
    //    }
    //}

    //void RollDice()
    //{
    //    if(!thrown && !hasLanded)
    //    {
    //        thrown = true;
    //        rb.useGravity = true;
    //        //transform.rotation = Quaternion.identity;
    //        rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
    //        rb.AddForce(new Vector3(0, 70, 0), ForceMode.Impulse);

    //    }
    //    else if(thrown && hasLanded)
    //    {
    //        DiceReset();
    //    }
    //}
    //void DiceReset()
    //{
    //    transform.position = initPostion;
    //    thrown = false;
    //    hasLanded = false;
    //    rb.useGravity = false;
    //}
    //void RollAgain()
    //{
    //    DiceReset();
    //    thrown = true;
    //    rb.useGravity = true;
    //    //rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
    //    rb.AddForce(new Vector3(0, 70, 0),ForceMode.Impulse);

    //}

    //void SideValueCheck()
    //{
    //    diceValue = 0;

    //    foreach (DiceSide item in diceSides)
    //    {
    //        if (item.OnGround)
    //        {
    //            diceValue = item.sideValue;
    //            Debug.Log("Dice Value " + diceValue);
    //        }
    //    }
    //}
    #endregion
}
