using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DisableAudioListener : NetworkBehaviour
{
   [SerializeField] private AudioListener audioListener;
    void Start()
    {
        Debug.Log("IS OWNER?????: " + IsOwner);

        Debug.Log("IS HOSST?????: " + IsHost);

        if (!IsOwner)
        {
            audioListener.enabled = false;
        }
    }

}
