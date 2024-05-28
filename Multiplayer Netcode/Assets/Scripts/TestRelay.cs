using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestRelay : MonoBehaviour{

    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private TMP_InputField inputField;

    private string joinCode = null;

    [SerializeField] private string showJoinCode;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        if (createButton != null)
        {
            createButton.onClick.AddListener(CreateRelay);
        }
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        if (joinButton != null)
        {
            joinButton.onClick.AddListener(JoinRelay);
        }

    }

    private void JoinGame()
    {
            //NetworkManager.Singleton.StartHost(); // Em vez de clicar no botão host, chmará por aqui

            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).completed += (operation) =>     //O código abaixo só vai executar quando a scene loadar toda
            {
                NetworkManager.Singleton.StartClient(); // Em vez de clicar no botão client, chmará por aqui
            };
    }

    //[SerializeField] private GameObject inputFieldObject;
    //[SerializeField] private InputField text;

    // Inicializando unity services
    // É async, pois manda um request par o unity services inicializar a api
    // e se n fosse asyn/await o jogo travaria até o jogo receber a resposta
    private async void  Start()                             
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {             // Inscrevendo no evento SignedIn para saber quando o player logará
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync(); // logando anonimamente// poderia ser com varias contas


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CreateRelay();
        }

        
    }

    private async void CreateRelay()
    {
        try
        { // Toda função de Relay tem exceção, tem q tratar para n travar o jogo
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3); //mas number of connectiso, except host


            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Join Code: " + joinCode);

            /// Antigo método
            ///NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData( // passando imformaçoes do server relay para o Unity Transport (HOST)
            ///        allocation.RelayServer.IpV4,
            ///        (ushort)allocation.RelayServer.Port,
            ///        allocation.AllocationIdBytes,
            ///        allocation.Key,
            ///        allocation.ConnectionData
            ///    );
            ///    
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls"); // udp, dtls. dtls é criptografado

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); // bem mais simples que o antigo. relayServerData terá todas as informações necessárias já
            if (codeText != null)
            {
            codeText.text = joinCode;
            }
            //NetworkManager.Singleton.StartHost(); // Em vez de clicar no botão host, chmará por aqui
            //StartGameScene();
            startButton.gameObject.SetActive(true); // Ativando botão depois de criar o código
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }

    public void StartGame()
    {
        if (joinCode != null)
        {
            //NetworkManager.Singleton.StartHost(); // Em vez de clicar no botão host, chmará por aqui

            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).completed += (operation) =>     //O código abaixo só vai executar quando a scene loadar toda
            {
                NetworkManager.Singleton.StartHost(); // Em vez de clicar no botão host, chmará por aqui
                showJoinCode = joinCode;
                createButton.onClick.RemoveAllListeners();
            };
        }
    }

    public async void JoinRelay() // Joinando o server com o joinCode gerado
    {
        try
        {
            if (inputField != null)
            {
                joinCode = inputField.text;
                Debug.Log("Join Relay with " + joinCode);
            }
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            /* Antigo método
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData( // passando imformaçoes do server relay para o Unity Transport (CLIENT)
                    joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData
                );
            */

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls"); // udp, dtls. dtls é criptografado

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); // bem mais simples que o antigo. relayServerData terá todas as informações necessárias já

            //NetworkManager.Singleton.StartClient(); // Em vez de clicar no botão client, chmará por aqui
            JoinGame();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    
}
