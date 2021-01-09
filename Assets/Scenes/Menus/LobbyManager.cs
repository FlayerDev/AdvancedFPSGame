using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NobleConnect.Mirror;
using Mirror;

public class LobbyManager : MonoBehaviour
{
    NobleNetworkManager networkManager;
    void Start() { 
        networkManager = (NobleNetworkManager)NetworkManager.singleton;
        networkManager.InitClient();
    }

    public InputField ipField;
    public InputField portField;
    public string IP = "";
    public string PORT = "";

    public LobbyState lobbyState = LobbyState.None;


    // Update is called once per frame
    void Update()
    {
        
    }
    public void GUI_Refresh()
    {
        IP = ipField.text;
        PORT = portField.text;
    }
    public void Lobby_Join()
    {
        networkManager.networkAddress = IP;
        networkManager.networkPort = ushort.Parse(PORT);
        networkManager.StartClient();
        lobbyState = LobbyState.Client;
    }
    public void Lobby_Host()
    {
        networkManager.StartHost();
        lobbyState = LobbyState.Host;
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
    }
}
public enum LobbyState
{
    None,
    Client,
    Host,
}
