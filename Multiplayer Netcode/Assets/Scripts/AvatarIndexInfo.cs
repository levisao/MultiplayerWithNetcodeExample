using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarIndexInfo : MonoBehaviour
{

    public static AvatarIndexInfo instance; //Singleton

    public Action<int> onAvatarIndexChange; //evento passando valor do avatarIndex


    private int avatarIndex = 1;

    public int AvatarIndex
    {
        get { return avatarIndex; }
    }
    void Awake()
    {
        DontDestroyOnLoad(this); //Singleton

        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetAvatarIndex(int index)
    {
        avatarIndex = index;

        onAvatarIndexChange(avatarIndex);
    }

    
    void Update()
    {
        
    }
}
