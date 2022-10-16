using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    public float force;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
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
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + " random number: " + newValue._int + " " + newValue._bool + " " + newValue.message);
        };
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        transform.Translate(Input.GetAxisRaw("Horizontal") * force, 0, Input.GetAxisRaw("Vertical") * force);

        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
                message = "wooo dit werkt",
            };
        }
    }
}
