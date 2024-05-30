using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarIndexInfo : MonoBehaviour
{

    public static AvatarIndexInfo instance; //Singleton

    public event Action<int> onAvatarIndexChange; //evento passando valor do avatarIndex


    private int avatarIndex = 3;

    public int AvatarIndex
    {
        get { return avatarIndex; }
    }
    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("CU CU CUC UCUC");
            //Singleton
            instance = this;
            DontDestroyOnLoad(this); 
        }


    }

    public void SetAvatarIndex(int index)
    {
        avatarIndex = index;

        onAvatarIndexChange?.Invoke(avatarIndex);
        //onAvatarIndexChange(avatarIndex);
    }

    
    void Update()
    {
        
    }
}
