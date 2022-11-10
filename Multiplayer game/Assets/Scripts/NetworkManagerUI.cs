using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;

public class NetworkManagerUI : NetworkBehaviour
{
    Dictionary<ulong, PlayerNetwork> clientControllers = new Dictionary<ulong, PlayerNetwork>();

    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button startBtnBtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI scorePlayerOne;
    [SerializeField] private TextMeshProUGUI scorePlayerTwo;
    [SerializeField] private GameObject inputField;
    [SerializeField] private GameObject startBtn;

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
            foreach (KeyValuePair<ulong, PlayerNetwork> entry in clientControllers)
            {
                entry.Value.gameStarted = true;
            }
            StartGameClientRpc();
        });
    }

    [ClientRpc]
    private void StartGameClientRpc()
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
                if (time == 0)
                {
                    Debug.Log("Game Over!");
                }
            }
        }
    }

    public void AddNewPlayer(PlayerNetwork _clientController, ulong _clientId)
    {
        clientControllers.Add(_clientId, _clientController);
        Debug.Log($"{_clientId} connected!");
    }
    [ServerRpc(RequireOwnership = false)]
    public void BallCollectedServerRpc(ulong playerID)
    {
        Debug.Log($"{playerID} collected a point!");
        clientControllers[playerID].UpdateScoreClientRpc(playerID);
    }
}
