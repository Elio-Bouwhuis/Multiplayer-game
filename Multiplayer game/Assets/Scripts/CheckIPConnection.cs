using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckIPConnection : MonoBehaviour
{

    [SerializeField] Unity.Netcode.Transports.UTP.UnityTransport unityTransport;
    [SerializeField] TextMeshProUGUI inputField;

    public void UpdateAdress()
    {
        unityTransport.ConnectionData.Address = inputField.text;
    }
}
