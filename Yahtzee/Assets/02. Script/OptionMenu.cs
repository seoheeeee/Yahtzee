using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviourPun
{
    enum Option
    {
        NewGame,
        ExitRoom
    }
    [SerializeField]
    Option option;

    [SerializeField]
    GameObject objMenu;

    [SerializeField]
    GameObject objYesOrNo;
    private void Start()
    {
        objMenu.SetActive(false);
        objYesOrNo.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!objYesOrNo.activeSelf)
                OnOff(objMenu);
            else
                OnOff(objYesOrNo);
        }
    }

    public void OnOff(GameObject obj) => obj.SetActive(!obj.activeSelf);
    public void OptionSelect(int select) => option = (Option)select;

    public void OnNewGame()
    {
        if (option == Option.NewGame)
            photonView.RPC("NewGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnExitRoom() 
    {
        if (option == Option.ExitRoom)
        {
            SceneManager.LoadScene(0);
        }
    }
}
