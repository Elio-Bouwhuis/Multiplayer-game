using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Net;
using System.Net.Sockets;

public class CheckIPConnection : MonoBehaviour
{

    [SerializeField] Unity.Netcode.Transports.UTP.UnityTransport unityTransport;
    [SerializeField] TextMeshProUGUI inputField;

    public void UpdateAdress()
    {
        IPHostEntry host;
        string localIP = "0.0.0.0";
        if (NetworkManager.Singleton.IsServer)
        {
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
        }

        Debug.Log(localIP);
        string newIp = inputField.text;
        //unityTransport.ConnectionData.Address = inputField.text;
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            localIP,  // The IP address is a string
            (ushort)7777, // The port number is an unsigned short (ushort)12345
            "0.0.0.0"
        );
        Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            "192.168.2.14",  // The IP address is a string
            (ushort)7777 // The port number is an unsigned short (ushort)12345
        );
        }
    }*/
}
