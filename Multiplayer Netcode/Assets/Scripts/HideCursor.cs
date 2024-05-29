#if UNITY_STANDALONE_WIN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HideCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
    }

}
#endif
