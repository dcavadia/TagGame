using System.Collections;
using Unity.Netcode;
using UnityEngine;


/// <summary>
/// Character controller for movement
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject targetCM;

    private void Start()
    {
        SetGameplayCamera();
    }

    public void SetGameplayCamera()
    {
        TagGameManager.Instance.gameplayCamera.Follow = targetCM.transform;
        TagGameManager.Instance.gameplayCamera.LookAt = targetCM.transform;
        TagGameManager.Instance.introCamera.gameObject.SetActive(false);
    }
}