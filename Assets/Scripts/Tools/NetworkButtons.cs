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

    /*private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        }

        GUILayout.EndArea();
    }*/

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

    //Artificial lag:
    // private void Awake() {
    //     GetComponent<UnityTransport>().SetDebugSimulatorParameters(
    //         packetDelay: 120,
    //         packetJitter: 5,
    //         dropRate: 3);
    // }
}