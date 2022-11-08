using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class CheckIPConnection : NetworkBehaviour
{

    [SerializeField] Unity.Netcode.Transports.UTP.UnityTransport unityTransport;
    [SerializeField] TextMeshProUGUI inputField;

    public void UpdateAdress()
    {
        //unityTransport.ConnectionData.Address = inputField.text;
        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetConnectionData(
            inputField.text,  // The IP address is a string
            7777, // The port number is an unsigned short (ushort)12345
            "0.0.0.0" // The server listen address is a string.
        );
    }
}
