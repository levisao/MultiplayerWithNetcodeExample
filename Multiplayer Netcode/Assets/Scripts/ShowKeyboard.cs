using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class ShowKeyboard : MonoBehaviour
{
    private TMP_InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener((stg) => GetOpenKeyboard()); //o onSelect requer q se passe uma string, então criamos uma default ai
    }

    private void GetOpenKeyboard()
    {
        NonNativeKeyboard.Instance.InputField = inputField; //setando o input field deles lá para o meu
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
