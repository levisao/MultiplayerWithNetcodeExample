using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    void Start()
    {
#if UNITY_STANDALONE_WIN

        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("HIIIIIIIIIIIIIIII");
        Cursor.visible = false;
    }
#endif
}
