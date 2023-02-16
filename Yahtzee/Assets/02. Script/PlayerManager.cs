using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    int num;
    
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine)
            photonView.RPC("SetPlayer", 
                RpcTarget.AllBuffered, 
                PhotonNetwork.PlayerList.Length);
    }

    [PunRPC]
    void SetPlayer(int num)
    {
        this.num = num;
    }
}
