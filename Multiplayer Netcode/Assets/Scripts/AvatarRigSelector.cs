using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarRigSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] avatarPrefabs;

    [SerializeField] private AvatarSelectionManager avatarSelectionManager; // se precisar mudar o personagem mid game, usar um outro script dondestroyonload ou fazer um script só para o index?
                                                                            // fazer um singleton com o evento

    [SerializeField] Transform xrRig;

    AvatarInputConverter avatarInputConverter;

    private void OnEnable()
    {
        if (avatarSelectionManager != null)
        {
            avatarSelectionManager.onAvatarIndexChange += ChangeAvatar;
        }
    }

    private void Start()
    {
        avatarInputConverter = xrRig.GetComponent<AvatarInputConverter>();
    }

    private void ChangeAvatar(int avatarIndex)
    {
        for (int i = 0; i < avatarPrefabs.Length; i++)
        {
            avatarPrefabs[i].SetActive(i == avatarIndex);

            if (i == avatarIndex)
            {
                avatarInputConverter.AvatarBody = transform.Find(avatarPrefabs[i].name).GetChild(0);
                avatarInputConverter.AvatarHand_Left = transform.Find(avatarPrefabs[i].name).GetChild(1);
                avatarInputConverter.AvatarHand_Right = transform.Find(avatarPrefabs[i].name).GetChild(2);
                avatarInputConverter.AvatarHead = transform.Find(avatarPrefabs[i].name).GetChild(3);

            }
        }
    }
}
