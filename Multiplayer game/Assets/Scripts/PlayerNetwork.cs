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
    public ulong networkObjectId;

    public float force;

    public bool gameStarted = false;

    [SerializeField] TextMeshProUGUI gameTimer;

    private int time = 300;
    private float timePassed = 0f;

    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform;

    //public AudioClip coinSound;

    public NetworkVariable<int> playerScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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
        transform.Translate(Input.GetAxisRaw("Horizontal") * force, 0, Input.GetAxisRaw("Vertical") * force);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            serverManager.BallCollectedServerRpc(player, networkObjectId);
            networkObjectId = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            other.gameObject.SetActive(false);
            //AudioSource.PlayClipAtPoint(coinSound,transform.position,1);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TestServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        Debug.Log(clientId);
    }

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
        if(playerID == OwnerClientId)
        {
            playerScore.Value += 1;
            Debug.Log("sus");
        }
    }

    [ClientRpc]
    public void StartGameClientRpc()
    {
        gameStarted = true;
        GameObject.Find("NetworkManagerUI").GetComponent<NetworkManagerUI>().StartGameClientRpc();
    }

}
