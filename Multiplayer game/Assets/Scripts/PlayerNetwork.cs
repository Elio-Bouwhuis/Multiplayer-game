using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using TMPro;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{
    public float force;

    //private int time = 300;
    //private float timePassed = 0f;

    //private bool toggle = false;

    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform;

    [SerializeField] TextMeshProUGUI scorePlayerOne;
    [SerializeField] TextMeshProUGUI scorePlayerTwo;
    //[SerializeField] TextMeshProUGUI gameTimer;

    //[SerializeField] private Button startBtnBtn;
    //[SerializeField] private GameObject startBtn;

    public NetworkVariable<int> playerScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    /*private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData {
            _int = 56,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }*/

    public override void OnNetworkSpawn()
    {
        scorePlayerOne = GameObject.Find("ScorePlayerOne").GetComponent<TextMeshProUGUI>();
        playerScore.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + " " + playerScore.Value);
            scorePlayerOne.text = "Player 1: " + playerScore.Value;
            //TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } });
            //TestServerRpc(new ServerRpcParams());
        };
        /*randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + " random number: " + newValue._int + " " + newValue._bool + " " + newValue.message);
        };*/
    }
    private void Start()
    {
        AfterClientSetupServerRpc();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            
            //playerScore.Value += 1;
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            //TestClientRpc();
            /*randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
                message = "wooo dit werkt",
            };*/
        }
        transform.Translate(Input.GetAxisRaw("Horizontal") * force, 0, Input.GetAxisRaw("Vertical") * force);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {

           // spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
            NetworkManagerUI lobbyManager = GameObject.Find("NetworkManagerUI").GetComponent<NetworkManagerUI>();
            lobbyManager.BallCollectedServerRpc(OwnerClientId); ;
            Debug.Log("ball destoyed");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TestServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        Debug.Log(clientId);

        //Debug.Log("test server rpc " + OwnerClientId + " " + serverRpcParams.Receive.SenderClientId);
    }

    /*[ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        //scorePlayerTwo = GameObject.Find("ScorePlayerTwo").GetComponent<TextMeshProUGUI>();
        //scorePlayerTwo.text = "Player 2: " + playerScore.Value;
        Debug.Log("TestClientRpc");
    }*/

    [ServerRpc (RequireOwnership = false)]
    public void AfterClientSetupServerRpc()
    {
        NetworkManagerUI lobbyManager = GameObject.Find("NetworkManagerUI").GetComponent<NetworkManagerUI>();
        lobbyManager.AddNewPlayer(this, OwnerClientId);
        Debug.Log($"Clientcontroller for clientID {OwnerClientId} succesfully setup!");
    }

    [ClientRpc]
    public void UpdateScoreClientRpc(ulong playerID)
    {
        Debug.Log($"{OwnerClientId} gained one point!");
        if(playerID == OwnerClientId) playerScore.Value += 1;
        else Debug.Log($"And that wasn't me :(");
    }

}
