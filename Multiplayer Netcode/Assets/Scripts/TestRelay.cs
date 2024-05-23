using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestRelay : MonoBehaviour{
    string joinCode;
                                 
                                 // Inicializando unity services
                                 // � async, pois manda um request par o unity services inicializar a api
                                 // e se n fosse asyn/await o jogo travaria at� o jogo receber a resposta
    private async void  Start()                             
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {             // Inscrevendo no evento SignedIn para saber quando o player logar�
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

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (joinCode != null) // jeito armengado, s� para testar
                                  // a forma certa � o client digitar o c�digo
            {
                JoinRelay(joinCode);
            }
            else
            {
                Debug.Log("N�o tem c�digo, mano");
            }
        }
    }

    private async void CreateRelay()
    {
        try { // Toda fun��o de Relay tem exce��o, tem q tratar para n travar o jogo
            Allocation allocation =  await RelayService.Instance.CreateAllocationAsync(3); //mas number of connectiso, except host

            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Join Code: " + joinCode);
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
            await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    
}
