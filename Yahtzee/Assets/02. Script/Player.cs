using JetBrains.Annotations;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    public int num;
    public bool isTurn;
    public Photon.Realtime.Player player;

    public void SetPlayer(int num, string name) => photonView.RPC("RPCSetPlayer", RpcTarget.AllBuffered, num, name);

    [PunRPC]
    void RPCSetPlayer(int num, string name)
    {
        this.num = num;
        this.name = name;
    }
}
