using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AvatarSelectionManager : MonoBehaviour
{
    /// <summary>
    /// public event EventHandler OnAvatarIndexChange;
    /// </summary>

    [SerializeField] private GameObject[] avatarPrefabs;

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    private int avatarIndex = 1;

    private void Awake()
    {

        previousButton.onClick.AddListener(() =>
        {
            avatarIndex--;

            if (avatarIndex < 0)
            {
                avatarIndex = avatarPrefabs.Length - 1;
            }

            SearchAndSelectAvatar();

        }
        );

        nextButton.onClick.AddListener(() =>
        {
            avatarIndex++;

            if (avatarIndex > avatarPrefabs.Length - 1)
            {
                avatarIndex = 0;
            }

            SearchAndSelectAvatar();
        }
        );

    }

    private void SearchAndSelectAvatar()
    {
        //Fire_OnIndexChangeEvent();

        for (int i = 0; i < avatarPrefabs.Length; i++)
        {
            if (i != avatarIndex)
            {
                avatarPrefabs[i].SetActive(false);
            }
            else
            {
                avatarPrefabs[i].SetActive(true);
            }
        }
    }

    private void Fire_OnIndexChangeEvent()
    {
        //OnAvatarIndexChange?.Invoke(this, EventArgs.Empty);
    }

    void Update()
    {

    }
}
