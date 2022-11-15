using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class CheckIPConnection : MonoBehaviour
{

    [SerializeField] Unity.Netcode.Transports.UTP.UnityTransport unityTransport;
    [SerializeField] TextMeshProUGUI inputField;

    public void UpdateAdress()
    {
        string newIp = inputField.text;
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            newIp,  // The IP address is a string
            (ushort)7777, // The port number is an unsigned short (ushort)12345
            "0.0.0.0"
        );
        Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);
    }
}
