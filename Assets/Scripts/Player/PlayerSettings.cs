using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    public List<Color> colors = new List<Color>();

    private void Awake()
    {
    }

    public override void OnNetworkSpawn()
    {
        meshRenderer.material.color = colors[(int)OwnerClientId]; 
    }
}
