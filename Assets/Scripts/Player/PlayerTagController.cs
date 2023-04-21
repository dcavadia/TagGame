using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

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
            crown.SetActive(true);
        }
        else
        {
            crown.SetActive(false);
        }
    }

    public override void OnNetworkSpawn()
    {
        //Debug.Log("SPAWN: " + OwnerClientId);
        if (NetworkObjectId == 1)
        {//Temporal way to get the first player to enter the match
            //Host mode
            crown.SetActive(true);
        }
        else if(NetworkObjectId == 2)
        {
            //Server mode
            //crown.SetActive(true);
        }
        else
        {

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
}