using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using TMPro;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{
    ServerManager serverManager;

    public int player;

    public float force;

    public bool gameStarted = false;

    [SerializeField] TextMeshProUGUI gameTimer;

    private int time = 300;
    private float timePassed = 0f;

    //private bool toggle = false;

    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform;

    // [SerializeField] TextMeshProUGUI scorePlayerOne;
    // [SerializeField] TextMeshProUGUI scorePlayerTwo;

    public AudioClip coinSound;

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

    [ClientRpc]
    public void SetPlayerNumberClientRpc(int _player)
    {
        this.player = _player;
    }

    public override void OnNetworkSpawn()
    {
        gameTimer = GameObject.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
        serverManager = GameObject.Find("Servermanager").GetComponent<ServerManager>();
        serverManager.AddNewPlayer(this, OwnerClientId);
        time = 300;
        timePassed = 0f;
        // scorePlayerOne = GameObject.Find("ScorePlayerOne").GetComponent<TextMeshProUGUI>();
        // scorePlayerTwo = GameObject.Find("ScorePlayerTwo").GetComponent<TextMeshProUGUI>();
        /*playerScore.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + " " + playerScore.Value);
            if(OwnerClientId == 0)
            {
                scorePlayerOne.text = "Player 1: " + playerScore.Value;
                Debug.Log("PLAYER 1 ball destoyed");
            }
            else if(OwnerClientId == 1)
            {
                scorePlayerTwo.text = "Player 2: " + playerScore.Value;
                Debug.Log("PLAYER 2 ball destoyed");
            }
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
        // AfterClientSetupServerRpc();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        timePassed += Time.deltaTime;

        if (timePassed > 1.0f && gameStarted == true)
        {
            time -= 1;
            timePassed = 0f;
            if(time < 300)
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    Vector3 randomSpawnPosition = new Vector3(Random.Range(-15, 9), 0, Random.Range(-3, 18));
                    spawnedObjectTransform = Instantiate(spawnedObjectPrefab, randomSpawnPosition, Quaternion.identity);
                    spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
                }
                Debug.Log(time);
                gameTimer.text = "Time left: " + time;
                if (time <= 0)
                {
                    GameObject.Find("QuitBtn").SetActive(true);
                    Debug.Log("Game Over!");
                    Time.timeScale = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //playerScore.Value += 1;
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
            Debug.Log("tedsta");
            //spawnedObjectTransform.GetComponent<NetworkObject>().networkid
            serverManager.BallCollectedServerRpc(player);//, networkid/networkobjectid
            Debug.Log("tedst3112da");
            AudioSource.PlayClipAtPoint(coinSound,transform.position,1);
            spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
            Debug.Log("tedst3112da31231d");
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
        // NetworkManagerUI lobbyManager = GameObject.Find("NetworkManagerUI").GetComponent<NetworkManagerUI>();
       // serverManager.AddNewPlayerServerRpc(this, OwnerClientId);
       // Debug.Log($"Clientcontroller for clientID {OwnerClientId} succesfully setup!");
    }

    [ClientRpc]
    public void UpdateScoreClientRpc(ulong playerID)
    {
        //Debug.Log($"{OwnerClientId} gained one point!");
        if(playerID == OwnerClientId)
        {
            playerScore.Value += 1;
            Debug.Log("sus");
        }
        //Debug.Log(playerScore.Value);
    }

    [ClientRpc]
    public void StartGameClientRpc()
    {
        gameStarted = true;
        GameObject.Find("NetworkManagerUI").GetComponent<NetworkManagerUI>().StartGameClientRpc();
    }

}
