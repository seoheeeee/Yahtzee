using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public int num;
    public bool isTurn;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetPlayer",
                RpcTarget.AllBuffered,
                PhotonNetwork.PlayerList.Length,
                PhotonNetwork.LocalPlayer.NickName);
        }
    }
    [PunRPC]
    void SetPlayer(int num, string name)
    {
        this.num = num;
        gameObject.name = name;
    }
}
