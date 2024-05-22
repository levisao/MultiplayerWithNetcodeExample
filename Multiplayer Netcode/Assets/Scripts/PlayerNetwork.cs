using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    void Update()
    {
        //Debug.Log("hey");
        // if player is not the owner ob the object, return
        if (!IsOwner) return; // Vem da classe NetworkBehaviour
        
        
        Vector3 moveDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDirection.z += 1;
        if (Input.GetKey(KeyCode.S)) moveDirection.z -= 1;
        if (Input.GetKey(KeyCode.A)) moveDirection.x -= 1;
        if (Input.GetKey(KeyCode.D)) moveDirection.x += 1;

        moveDirection.Normalize();

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

    }
}
