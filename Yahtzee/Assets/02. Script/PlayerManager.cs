using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    [SerializeField]
    TMP_Text numTxt;
    [SerializeField]
    int num;
    public int step;

    [SerializeField]
    float jumpHeight;
    [SerializeField]
    float offset;
    Vector3 jumpPos;
    Vector3 offsetPos;

    [Range(0,1)]
    [SerializeField]
    float speed;
    [SerializeField]
    float delay;

    public bool isMove;
    public bool isFork;
    public Node node;

    public int Num 
    { 
        get => num;
        set 
        {
            num = value;
            numTxt.text = value.ToString();
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        jumpHeight = 1.5f;
        offset = 0.75f;
        delay = 0.2f;
        jumpPos = new Vector3(0, jumpHeight, 0);
        offsetPos = new Vector3(0, offset, 0);
    }
    private void Update()
    {
        
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine)
            photonView.RPC("SetPlayer", RpcTarget.AllBuffered, PhotonNetwork.PlayerList.Length);
    }
    [PunRPC]
    void SetPlayer(int num)
    {
        Num = num;
    }
    public void Move(int diceValue, Direction direction = Direction.Straight , float speed = 1)
    {
        step = diceValue;
        StartCoroutine(MovePlayer(diceValue, direction, speed));
    }
    IEnumerator MovePlayer(int diceValue , Direction direction ,float speed)
    {
        for (int i = 0; i < diceValue; i++)
        {
            if (node.nextNode.Count >= 2)
            {
                step -= i;
                isFork = true;
                break;
            }
            Node nextNode = node.nextNode[direction];

            Vector3 start = node.transform.position + offsetPos;
            Vector3 end = nextNode.transform.position + offsetPos;

            while (1 + delay > this.speed)
            {
                this.speed += Time.deltaTime * speed;

                transform.position = Bezier(start, end, this.speed);

                yield return null;
            }
            node = nextNode;
            this.speed = 0;
        }
    }
    Vector3 Bezier(Vector3 start, Vector3 end, float value)
    {
        Vector3 startH = start + jumpPos;
        Vector3 endH = end + jumpPos;

        Vector3 A = Vector3.Lerp(start, startH, value);
        Vector3 B = Vector3.Lerp(startH, endH, value);
        Vector3 C = Vector3.Lerp(endH, end, value);

        Vector3 D = Vector3.Lerp(A, B, value);
        Vector3 E = Vector3.Lerp(B, C, value);

        Vector3 F = Vector3.Lerp(D, E, value);
        return F;
    }

    public void Teleport(Vector3 targetPos)
    {
        transform.position = targetPos + offsetPos;
    }
    //[PunRPC]
    //void TeleportRPC(Vector3 targetPos)
    //{
    //}

}
