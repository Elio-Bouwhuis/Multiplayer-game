using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using TMPro;

public class PlayerNetwork : NetworkBehaviour
{
    public float force;

    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform;

    [SerializeField] private TextMeshProUGUI scorePlayerOne;
    [SerializeField] private TextMeshProUGUI scorePlayerTwo;

    private NetworkVariable<int> playerScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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
        playerScore.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + " " + playerScore.Value);
            scorePlayerOne.text = "Player 1: " + playerScore.Value;
        };
        /*randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + " random number: " + newValue._int + " " + newValue._bool + " " + newValue.message);
        };*/
    }

    private void Update()
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
            playerScore.Value += 1;
            spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
            Debug.Log("ball destoyed");
        }
    }

    /*[ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("test server rpc " + OwnerClientId + " " + serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void TestClientRpc()
    {
        Debug.Log("TestClientRpc");
    }*/
}
