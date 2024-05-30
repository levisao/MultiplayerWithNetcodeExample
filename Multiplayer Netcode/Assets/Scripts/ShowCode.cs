using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowCode : MonoBehaviour
{
    void Start()
    {
        string joinCode = TestRelay.instance.JoinCode;
        
        Debug.Log("JoinCode: " + joinCode);
        gameObject.GetComponent<TextMeshProUGUI>().text = joinCode;
    }
}
