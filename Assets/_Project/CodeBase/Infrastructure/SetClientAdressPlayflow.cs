using System;
using FishNet.Object;
using FishNet.Transporting.Bayou;
using UnityEngine;

public class SetClientAdressPlayflow : NetworkBehaviour
{
    public Bayou bayouTransport;
    public LobbyHelloWorld lobbyHelloWorld;

    private void Start()
    {
        lobbyHelloWorld.DoCreateLobbyOnClick();
    }
}
