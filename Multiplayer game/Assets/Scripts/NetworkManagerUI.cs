using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI scorePlayerOne;
    [SerializeField] private TextMeshProUGUI scorePlayerTwo;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            Destroy(hostBtn.gameObject);
            Destroy(serverBtn.gameObject);
            Destroy(clientBtn.gameObject);
            Destroy(backgroundImage.gameObject);
            scorePlayerOne.enabled = true;
            scorePlayerTwo.enabled = true;
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Destroy(hostBtn.gameObject);
            Destroy(serverBtn.gameObject);
            Destroy(clientBtn.gameObject);
            Destroy(backgroundImage.gameObject);
            scorePlayerOne.enabled = true;
            scorePlayerTwo.enabled = true;
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            Destroy(hostBtn.gameObject);
            Destroy(serverBtn.gameObject);
            Destroy(clientBtn.gameObject);
            Destroy(backgroundImage.gameObject);
            scorePlayerOne.enabled = true;
            scorePlayerTwo.enabled = true;
        });
    }
}
