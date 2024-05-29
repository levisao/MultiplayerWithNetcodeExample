using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowCode : MonoBehaviour
{
    TestRelay relay;

    GameObject network;
    void Start()
    {
        network = GameObject.Find("Network Manager");

        relay = network.GetComponent<TestRelay>();

        
        Debug.Log("JoinCode: " + relay.JoinCode);
        gameObject.GetComponent<TextMeshProUGUI>().text = relay.JoinCode;
    }
    void Update()
    {
        
    }
}
