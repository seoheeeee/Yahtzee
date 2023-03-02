using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public int num;
    public bool isTurn;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

        if (GameManager.Instance != null)
        {
            if (photonView.IsMine)
            {
                if (isTurn)
                {
                    GameManager.Instance.Turn(false);
                    GameManager.Instance.Turn(true, num);
                }
                else
                {
                    GameManager.Instance.Turn(false);
                }
            }
        }
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetPlayer",
                RpcTarget.AllBuffered,
                PhotonNetwork.PlayerList.Length);
            gameObject.name = "Player" + num.ToString();
        }
    }
    [PunRPC]
    void SetPlayer(int num) => this.num = num;
}
