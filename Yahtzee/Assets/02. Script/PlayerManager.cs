using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public int num;
    public int turn;
    public bool isTurn;

    private void Awake()
    {
        turn = 13;
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
                    GameManager.Instance.PreviewScore(num);
                    GameManager.Instance.startBnt.gameObject.SetActive(true);
                    foreach (var item in GameManager.Instance.spriteManager)
                        item.button.enabled = true;
                }
                else
                {
                    GameManager.Instance.startBnt.gameObject.SetActive(false);
                    foreach (var item in GameManager.Instance.spriteManager)
                        item.button.enabled = false;
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
