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
    public CinemachineFreeLook gameplayCamera;
    public TagMatchNetwork TagMatchNetwork;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
