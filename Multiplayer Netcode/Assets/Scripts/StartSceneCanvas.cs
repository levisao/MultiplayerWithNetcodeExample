using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneCanvas : MonoBehaviour
{
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwtichToJoinCanvas()
    {
        GameObject.Find("Create Screen").SetActive(!active);
    }
}
