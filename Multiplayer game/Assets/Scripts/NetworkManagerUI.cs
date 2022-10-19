using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Image backgroundImage;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            Destroy(hostBtn.gameObject);
            Destroy(serverBtn.gameObject);
            Destroy(clientBtn.gameObject);
            Destroy(backgroundImage.gameObject);
        });
        hostBtn.onClick.AddListener(() =>
        {
            //NetworkManager.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            NetworkManager.Singleton.StartHost();
            Destroy(hostBtn.gameObject);
            Destroy(serverBtn.gameObject);
            Destroy(clientBtn.gameObject);
            Destroy(backgroundImage.gameObject);
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            Destroy(hostBtn.gameObject);
            Destroy(serverBtn.gameObject);
            Destroy(clientBtn.gameObject);
            Destroy(backgroundImage.gameObject);
        });
    }
}
