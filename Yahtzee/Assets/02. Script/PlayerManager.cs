using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public int num;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

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
