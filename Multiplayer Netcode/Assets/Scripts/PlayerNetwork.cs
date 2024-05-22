using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    // NetworkVariable � uma vari�vel que ser� passada para toda a network
    // Sempre deve ser inicializada
    // A sintaxe � assim: O (1) � o valor passado
    [SerializeField] private NetworkVariable<int> randomNumber1 = new NetworkVariable<int>(1);

    // Para permitir que todos na network modifiquem a vari�vel: ---------------------------------------------------------------------------------------------- **Aqui**
    [SerializeField] private NetworkVariable<int> randomNumber2 = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private float moveSpeed = 5f;


    // Basicamente o OnNetworkSpawn � o OnEnable/Start, deve-se usar ele quando trabalhando com network
    public override void OnNetworkSpawn()
    {
        // Inscrevendo uma fun��o ao "evento" OnValueChanged do NetworkVariable
        // Neste caso, uma fun��o lambda
        randomNumber2.OnValueChanged +=  (int previousValue, int newValue) => {  // Toda vez que o valor da vari�vel mudar, ser� chamada a fun��o inscrita
            Debug.Log(OwnerClientId + "; randomNumber: " + randomNumber2.Value);
        };
    }

    void Update()
    {
        //Debug.Log(OwnerClientId + "; randomNumber: " + randomNumber2.Value);

        //Debug.Log("hey");
        // if player is not the owner ob the object, return
        if (!IsOwner) return; // Vem da classe NetworkBehaviour

        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber2.Value = Random.Range(0,100);
            // .Value pq � assim mesmo que pega s� o valor da vari�vel
        }
        
        
        Vector3 moveDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDirection.z += 1;
        if (Input.GetKey(KeyCode.S)) moveDirection.z -= 1;
        if (Input.GetKey(KeyCode.A)) moveDirection.x -= 1;
        if (Input.GetKey(KeyCode.D)) moveDirection.x += 1;

        moveDirection.Normalize();

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

    }
}
