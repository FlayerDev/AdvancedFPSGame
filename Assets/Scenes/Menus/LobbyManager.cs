using Mirror;
using NobleConnect.Mirror;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    NobleNetworkManager networkManager;
    void Start()
    {
        networkManager = (NobleNetworkManager)NetworkManager.singleton;
        networkManager.InitClient();
    }
    public GameObject MainHUD;
    public GameObject LobbyHUD;

    public InputField ipField;
    public InputField portField;
    public Text ipText;
    public Text portText;
    public string IP = "";
    public string PORT = "";

    public LobbyState lobbyState = LobbyState.None;


    // Update is called once per frame
    void Update()
    {
        if (networkManager.HostEndPoint != null)
        {
            ipText.text = $"IP: {networkManager.HostEndPoint.Address}";
            portText.text = $"PORT: {networkManager.HostEndPoint.Port}";
        }
    }
    public void GUI_Refresh()
    {
        IP = ipField.text;
        PORT = portField.text;
    }
    public void GUI_State(bool isConnected)
    {
        MainHUD.SetActive(!isConnected);
        LobbyHUD.SetActive(isConnected);
    }
    public void Lobby_Join()
    {
        networkManager.networkAddress = IP;
        networkManager.networkPort = ushort.Parse(PORT);
        networkManager.StartClient();
        lobbyState = LobbyState.Client;
        GUI_State(true);
    }
    public void Lobby_Host()
    {
        networkManager.StartHost();
        lobbyState = LobbyState.Host;
        GUI_State(true);
    }
    public void Lobby_Leave()
    {
        switch (lobbyState)
        {
            case LobbyState.None:
                break;
            case LobbyState.Client:
                networkManager.StopClient();
                lobbyState = LobbyState.None;
                break;
            case LobbyState.Host:
                networkManager.StopHost();
                lobbyState = LobbyState.None;
                break;
            default:
                break;
        }
        GUI_State(false);
    }
}
public enum LobbyState
{
    None,
    Client,
    Host,
}
