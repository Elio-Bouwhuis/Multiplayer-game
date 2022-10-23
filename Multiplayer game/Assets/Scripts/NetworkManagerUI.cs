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
    [SerializeField] private Button startBtnBtn;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI scorePlayerOne;
    [SerializeField] private TextMeshProUGUI scorePlayerTwo;
    [SerializeField] private GameObject inputField;
    [SerializeField] private GameObject startBtn;

    [SerializeField] TextMeshProUGUI gameTimer;

    private int time = 300;
    private float timePassed = 0f;

    private void Awake()
    {
        time = 300;
        timePassed = 0f;
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            Destroy(hostBtn.gameObject);
            Destroy(serverBtn.gameObject);
            Destroy(clientBtn.gameObject);
            Destroy(backgroundImage.gameObject);
            Destroy(inputField.gameObject);
            scorePlayerOne.enabled = true;
            scorePlayerTwo.enabled = true;
            startBtn.SetActive(true);
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Destroy(hostBtn.gameObject);
            Destroy(serverBtn.gameObject);
            Destroy(clientBtn.gameObject);
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
            Destroy(serverBtn.gameObject);
            Destroy(clientBtn.gameObject);
            Destroy(backgroundImage.gameObject);
            Destroy(inputField.gameObject);
            scorePlayerOne.enabled = true;
            scorePlayerTwo.enabled = true;
            startBtn.SetActive(true);
        });
        startBtnBtn.onClick.AddListener(() =>
        {
            Debug.Log("start button clicked");
            gameTimer.enabled = true;
            //gameTimer = GameObject.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
            Destroy(startBtn.gameObject);
        });
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 1.0f)
        {
            time -= 1;
            timePassed = 0f;
            Debug.Log(time);
            //gameTimer.text = "Time left: " + i;
            if(time == 0)
            {
                Debug.Log("Game Over!");
            }
        }
    }
}
