using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class CheckIPConnection : MonoBehaviour
{

    [SerializeField] Unity.Netcode.Transports.UTP.UnityTransport unityTransport;
    [SerializeField] TextMeshProUGUI inputField;

    void UpdateAdress()
    {
        unityTransport.ConnectionData.Address = inputField.text;
    }
}
