using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class TestRelay : MonoBehaviour{

    [SerializeField] private GameObject inputFieldObject;
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

        inputFieldObject.GetComponent<InputField>().onEndEdit.AddListener(joinCodeText => JoinRelay(joinCodeText));
        //text.onEndEdit.AddListener(joinCodeText => JoinRelay(joinCodeText));

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
        try { // Toda função de Relay tem exceção, tem q tratar para n travar o jogo
            Allocation allocation =  await RelayService.Instance.CreateAllocationAsync(3); //mas number of connectiso, except host


            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

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

            NetworkManager.Singleton.StartHost(); // Em vez de clicar no botão host, chmará por aqui
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }


    private async void JoinRelay(string joinCode) // Joinando o server com o joinCode gerado
    {
        try
        {
            Debug.Log("Join Relay with " + joinCode);
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

            NetworkManager.Singleton.StartClient(); // Em vez de clicar no botão client, chmará por aqui
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    
}
