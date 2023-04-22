using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager of the game
/// </summary>
public class TagGameManager : SingletonComponent<TagGameManager>
{
    public CinemachineFreeLook introCamera;
    public List<CinemachineFreeLook> gameplayCamera = new List<CinemachineFreeLook>();
    public CinemachineVirtualCamera onTargetCamera;
    public TagMatchNetwork TagMatchNetwork;

    public Dictionary<ulong, PlayerTagController> players = new Dictionary<ulong, PlayerTagController>();

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
