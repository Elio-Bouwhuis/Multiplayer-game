using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;
using System.Net;
using System.Net.Sockets;

public class NetworkManagerUI : NetworkBehaviour
{
    Dictionary<ulong, PlayerNetwork> clientControllers = new Dictionary<ulong, PlayerNetwork>();

    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button startBtnBtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button quitBtnBtn;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI scorePlayerOne;
    [SerializeField] private TextMeshProUGUI scorePlayerTwo;
    [SerializeField] private GameObject inputField;
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject quitBtn;

    [SerializeField] TextMeshProUGUI gameTimer;

    private int time = 300;
    private float timePassed = 0f;

    private bool toggle = false;

    private void Start()
    {
        gameTimer = GameObject.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
        time = 300;
        timePassed = 0f;
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();

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
            //string newIp = inputField.text;
            //unityTransport.ConnectionData.Address = inputField.text;
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                localIP,  // The IP address is a string
                (ushort)7777, // The port number is an unsigned short (ushort)12345
                "0.0.0.0"
            );
            Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);

            Destroy(hostBtn.gameObject);
            Destroy(clientBtn.gameObject);
            Destroy(saveBtn.gameObject);
            Destroy(backgroundImage.gameObject);
            Destroy(inputField.gameObject);
            scorePlayerOne.enabled = true;
            scorePlayerTwo.enabled = true;
            startBtn.SetActive(true);
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            Destroy(hostBtn.gameObject);
            Destroy(clientBtn.gameObject);
            Destroy(saveBtn.gameObject);
            Destroy(backgroundImage.gameObject);
            Destroy(inputField.gameObject);
            scorePlayerOne.enabled = true;
            scorePlayerTwo.enabled = true;
            startBtn.SetActive(true);
        });
        startBtnBtn.onClick.AddListener(() =>
        {
            GameObject.Find("Servermanager").GetComponent<ServerManager>().StartGame();
           
        });
        quitBtnBtn.onClick.AddListener(() =>
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
        });
    }

    [ClientRpc]
    public void StartGameClientRpc()
    {
        TimeCounter();
        Debug.Log("start button clicked");
        gameTimer.enabled = true;
        Destroy(startBtn);
    }

    private void TimeCounter()
    {
        toggle = !toggle;
    }

    private void Update()
    {
        if (toggle)
        {
            timePassed += Time.deltaTime;
            if (timePassed > 1.0f)
            {
                time -= 1;
                timePassed = 0f;
                //Debug.Log(time);
                gameTimer.text = "Time left: " + time;
                if (time <= 0)
                {
                    quitBtn.SetActive(true);
                    Debug.Log("Game Over!");
                    Time.timeScale = 0;
                }
            }
        }
    }

    /*public void AddNewPlayer(PlayerNetwork _clientController, ulong _clientId)
    {
        clientControllers.Add(_clientId, _clientController);
        Debug.Log($"{_clientId} connected!");
    }
    [ServerRpc(RequireOwnership = false)]
    public void BallCollectedServerRpc(ulong playerID)
    {
        Debug.Log($"{playerID} collected a point!");
        clientControllers[playerID].UpdateScoreClientRpc(playerID);
    }*/
}
