using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("ConnectPanel")]
    [SerializeField]
    GameObject connectPanel;
    public TMP_InputField NickNameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public TMP_InputField RoomInput;
    public TMP_Text LobbyInfoText;
    public Button[] CellBtn;
    public Button BackBtn;
    public Button NextBtn;

    [Header("ChatPanel")]
    public GameObject ChatPanel;
    public TMP_Text RoomInfoText;
    public TMP_Text[] ChatText;
    public TMP_InputField ChatInput;

    [Header("ETC")]
    public PhotonView PV;
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;
    [SerializeField]
    GameObject SceneBtn;

    private void Start()
    {
        SceneBtn.SetActive(false);
    }

    //방리스트 갱신
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        BackBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<TMP_Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }

    void Awake() => Screen.SetResolution(2880, 1440, false);

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        LobbyPanel.SetActive(true);
        ChatPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.AutomaticallySyncScene = true;
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        LobbyPanel.SetActive(false);
        ChatPanel.SetActive(false);
    }

    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 2 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        ChatPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        connectPanel.SetActive(false);
        SceneBtn.SetActive(true);

        RoomRenewal();
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";

        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }



    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    void RoomRenewal()
    {
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name +
            " / " + PhotonNetwork.CurrentRoom.PlayerCount +
            "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";
    }

    //채팅
    public void Send()
    {
        PV.RPC("ChatRPC", RpcTarget.AllBuffered, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC]
    void SceneChange()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.PlayerList.Length == 2)
            SceneManager.LoadScene(1);
        }
    }

    public void OnButtonSceneChange()
    {
        PV.RPC("SceneChange", RpcTarget.All);
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
        {
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }

    }



}
