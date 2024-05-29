using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    /// <summary>
    ///  Singleton que pega o valor do index do avatar do AvatarSelectionManager por meio de evento para passar para o XR Rig e trocar para o prefabo certo
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
