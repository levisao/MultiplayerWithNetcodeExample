using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    // NetworkVariable é uma variável que será passada para toda a network
    // Sempre deve ser inicializada
    // A sintaxe é assim: O (1) é o valor passado
    // TEM que ser Value type
    [SerializeField] private NetworkVariable<int> randomNumber1 = new NetworkVariable<int>(1);

    // Para permitir que todos na network modifiquem a variável: ---------------------------------------------------------------------------------------------- **Aqui**
    [SerializeField] private NetworkVariable<int> randomNumber2 = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform; // Referencia para o prefab de cima ^^^, que está sendo usada para chamar os métodos necessários

    // Se quiser uma variável que se assemelha a classe, use struct, pois é Value Type
    [SerializeField] private NetworkVariable<MyCustomData> randomValues = new NetworkVariable<MyCustomData>(
        new MyCustomData { 
            _int = 56,
            _bool = true,
            _message = "hey",
    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private struct MyCustomData : INetworkSerializable // Tem que implementar essa interface sempre quando usar com Custom Data (Struct)
    {
        public int _int;
        public bool _bool;
        // se quiser usar string, é preciso usar a FixedString128Bytes, pois não é reference Type
        public FixedString128Bytes _message; // para usar precisa do using Unity.Collections;
        // 1 chracter is 1 byte, so the maximum is a message with 128 characters 

        // Interface "gerada" pelo Quick Fix
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int); // Deve-se serializar os dois valores
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref _message);
        }
    }

    // Basicamente o OnNetworkSpawn é o OnEnable/Start, deve-se usar ele quando trabalhando com network
    public override void OnNetworkSpawn()
    {
        /// <param name="previousValue">The value before the change</param>
        /// <param name="newValue">The new value</param>
        // Inscrevendo uma função ao "evento" OnValueChanged do NetworkVariable
        // Neste caso, uma função lambda
        randomNumber2.OnValueChanged +=  (int previousValue, int newValue) => {  // Toda vez que o valor da variável mudar, será chamada a função inscrita
            Debug.Log(OwnerClientId + "; randomNumber: " + newValue);
        };

        randomValues.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + "; random int: " + newValue._int + "; Random Bool " + newValue._bool + "; message: " + newValue._message);
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
            randomNumber2.Value = Random.Range(0, 100);
            // .Value pq é assim mesmo que pega só o valor da variável

            randomValues.Value = new MyCustomData
            {
                _int = Random.Range(0, 90),
                _bool = false,
                _message = "Don't waste your time on me you're already the voice inside my head!!!!"
            };
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            TestServerRpc("I BELIEVE I CAN FLY");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            TestClientRpc();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Test2ClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } }); // Só executará no cliente 1
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            /// Instantiate(spawnedObjectPrefab); // normalmente seria assim
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true); // Para objetos de network deve chamar o Spawn(destroy with scene?) para aparecer para todos
                                                                              // Somente o Server/Host pode spawnar objetos
                                                                              // Se quiser que o cliente spawne (balas, projéteis...) deve-se usar um serverRpc
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Destroy(spawnedObjectTransform.gameObject); // Simples assim para destruir um network game object
            /// spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true); // se quiser manter o objeto criado, mas só o "esconder"
        }

        // Movimento do player
        HandleMovement();

    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDirection.z += 1;
        if (Input.GetKey(KeyCode.S)) moveDirection.z -= 1;
        if (Input.GetKey(KeyCode.A)) moveDirection.x -= 1;
        if (Input.GetKey(KeyCode.D)) moveDirection.x += 1;

        moveDirection.Normalize();

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    [ServerRpc]                                // O nome deve SEMPRE terminar com ServerRPC   
    private void TestServerRpc(string message) // O código da função SEMPRE irá ser executada no Host/Server mesmo sendo chamada do cliente
    {                                          // Sempre deve estar num NetworkBehaviour e attached to a gameobject
                                               // valores passados também devem ser VALUE TYPE, mas estranhamente pode usar STRING
        Debug.Log("Theres nothing else I can say!! EH EH " + OwnerClientId + " " + message);
    }

    [ClientRpc]
    private void TestClientRpc() // Client RPCs are meant to be called on the server/host and ran on the Client
    {                            // Client cannot call this function
        Debug.Log("O clienteeee");
    }

    [ClientRpc]
    private void Test2ClientRpc(ClientRpcParams clientRpcParams) //Default struct that gets things like list of Clients ids -- O do server é ServerRpcParams 
    {                                                           // can define something to run on a specific client
        Debug.Log("Rodando só em um cliente em neném " + OwnerClientId);
    }
}
