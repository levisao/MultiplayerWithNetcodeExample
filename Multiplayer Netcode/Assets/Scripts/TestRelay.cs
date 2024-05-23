using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class TestRelay : MonoBehaviour{
                                 
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
    
}
