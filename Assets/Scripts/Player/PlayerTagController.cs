using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using MalbersAnimations;

/// <summary>
/// Controller of the tag game mechanics in player
/// </summary>
public class PlayerTagController : NetworkBehaviour
{
    [SerializeField] private GameObject crown;

    private void Awake()
    {
        TagGameManager.Instance.TagMatchNetwork.whoHasCrown.OnValueChanged += OnValueChanged;
    }

    public override void OnDestroy()
    {
        if (TagGameManager.Instance.TagMatchNetwork) TagGameManager.Instance.TagMatchNetwork.whoHasCrown.OnValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(ulong prevCrownValue, ulong nextCrownValue)
    {
        if (nextCrownValue == OwnerClientId)
        {
            EnableCrown();
        }
        else
        {
            DisableCrown();
        }
    }

    public override void OnNetworkSpawn()
    {
        TagGameManager.Instance.players.Add(NetworkObjectId, this);

        if(TagGameManager.Instance.players.Count == 1)
        {
            EnableCrown();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (TagGameManager.Instance.TagMatchNetwork.inmunity.Value || !IsOwner) return;

        var enemy = collision.gameObject.GetComponent<PlayerTagController>();
        if (enemy)
        {
            if ((enemy.crown.activeSelf))
            {//Temporal way to know if a player have the crown or not
                var player1 = new TagMatchNetworkState()
                {
                    Id = OwnerClientId,
                    HasCrown = false
                };
                var player2 = new TagMatchNetworkState()
                {
                    Id = enemy.OwnerClientId,
                    HasCrown = true
                };
                TagGameManager.Instance.TagMatchNetwork.DetermineCollisionWinnnerServerRpc(player1, player2);
            }
            else if ((crown.activeSelf))
            {
                var player1 = new TagMatchNetworkState()
                {
                    Id = OwnerClientId,
                    HasCrown = true
                };
                var player2 = new TagMatchNetworkState()
                {
                    Id = enemy.OwnerClientId,
                    HasCrown = false
                };
                TagGameManager.Instance.TagMatchNetwork.DetermineCollisionWinnnerServerRpc(player2, player1);

            }

        }
    }

    bool HasCrown()
    {
        return NetworkObjectId == TagGameManager.Instance.TagMatchNetwork.whoHasCrown.Value;
    }

    void DisableCrown()
    {
        crown.SetActive(false);
    }

    void EnableCrown()
    {
        crown.SetActive(true);
    }
}