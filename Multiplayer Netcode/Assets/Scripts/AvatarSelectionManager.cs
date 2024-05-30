using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelectionManager : MonoBehaviour
{
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

    private void Start()
    {
        Debug.Log("LENGH: " + avatarPrefabs.Length);
    }
    void Update()
    {
        
    }
}
