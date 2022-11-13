using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ServerManager : NetworkBehaviour
{
    int playerOneScore, playerTwoScore;
    [SerializeField] TextMeshProUGUI scorePlayerOne;
    [SerializeField] TextMeshProUGUI scorePlayerTwo;
    Dictionary<ulong, PlayerNetwork> clientControllers = new Dictionary<ulong, PlayerNetwork>();

    // Start is called before the first frame update
    void Start()
    {
        clientControllers = new Dictionary<ulong, PlayerNetwork>();
        scorePlayerOne = GameObject.Find("ScorePlayerOne").GetComponent<TextMeshProUGUI>();
        scorePlayerTwo = GameObject.Find("ScorePlayerTwo").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddNewPlayer(PlayerNetwork _clientController, ulong _clientId)
    {
        clientControllers.Add(_clientId, _clientController);
        _clientController.SetPlayerNumberClientRpc(clientControllers.Count);
        Debug.Log($"{_clientId} connected!");
    }

    [ServerRpc]
    public void BallCollectedServerRpc(int player)
    {
        //Destroy that object met networkid
        if (player == 1) playerOneScore++;
        else playerTwoScore++;
        Debug.Log($"{player} collected a point!");
        UpdateScoresClientRpc(playerOneScore, playerTwoScore);
    }

    [ClientRpc]
    public void UpdateScoresClientRpc(int _playerOneScore, int _playerTwoScore)
    {
        scorePlayerOne.text = "Player 1: " + _playerOneScore;
        scorePlayerTwo.text = "Player 2: " + _playerTwoScore;
    }

    public void StartGame()
    {
        foreach (KeyValuePair<ulong, PlayerNetwork> entry in clientControllers)
        {
            
            entry.Value.StartGameClientRpc();
        }
    }

}
