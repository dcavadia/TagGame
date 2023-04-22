using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

/// <summary>
/// Network buttons helper
/// </summary>
public class NetworkButtons : MonoBehaviour
{
    public GameObject steveNpc;
    public GameObject mathcmakingPanel;
    bool matchStarted = false;

    private void Update()
    {
        if(!matchStarted)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                NetworkManager.Singleton.StartHost();
                matchStarted= true;
                mathcmakingPanel.SetActive(false);
                steveNpc.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                NetworkManager.Singleton.StartServer();
                matchStarted = true;
                mathcmakingPanel.SetActive(false);
                steveNpc.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                NetworkManager.Singleton.StartClient();
                matchStarted = true;
                mathcmakingPanel.SetActive(false);
                steveNpc.SetActive(false);
            }
        }
        
    }
}