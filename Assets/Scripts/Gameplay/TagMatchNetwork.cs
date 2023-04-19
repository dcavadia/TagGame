using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Controller of the current tag match 
/// </summary>
public class TagMatchNetwork : NetworkBehaviour
{
    [HideInInspector]
    public readonly NetworkVariable<ulong> whoHasCrown = new(999, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector]
    public readonly NetworkVariable<bool> inmunity = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private readonly ulong[] targetClientsArray = new ulong[1];

    private void Awake()
    {
        inmunity.OnValueChanged += OnValueChanged;
    }

    public override void OnDestroy() => inmunity.OnValueChanged -= OnValueChanged;

    private void OnValueChanged(bool prevCrownValue, bool nextCrownValue)
    {
        inmunity.Value = nextCrownValue;
        if (nextCrownValue)
        {
            //Debug.Log("INMUNITY ENABLED!");
            StartCoroutine(DisableInmunityCollisionTimer());

        }
        else
        {
            //Debug.Log("INMUNITY DISABLE!");
        }

    }

    public IEnumerator DisableInmunityCollisionTimer()
    {
        yield return new WaitForSeconds(3f);
        InmunityCollisionServerRpc(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DetermineCollisionWinnnerServerRpc(TagMatchNetworkState player1, TagMatchNetworkState player2)
    {
        if (player2.HasCrown)
        {
            whoHasCrown.Value = player1.Id;
            inmunity.Value = true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void InmunityCollisionServerRpc(bool inmunityState)
    {
        inmunity.Value = inmunityState;
        Debug.Log("State: "+ inmunity.Value);
    }

    [ServerRpc]
    private void WinLoseInformationServerRpc(ulong winner, ulong loser)
    {
        targetClientsArray[0] = winner;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = targetClientsArray
            }
        };

        WinCrownClientRpc(clientRpcParams);

        targetClientsArray[0] = loser;
        LoseCrownClientRpc(clientRpcParams);
    }

    [ClientRpc]
    private void WinCrownClientRpc(ClientRpcParams clientRpcParams = default)
    {
        if (!IsOwner) return;       
        //Show winning effects on client x     
    }

    [ClientRpc]
    private void LoseCrownClientRpc(ClientRpcParams clientRpcParams = default)
    {
        if (!IsOwner) return;
        //Show losing effects on client x
    }
}

public struct TagMatchNetworkState : INetworkSerializable
{
    private ulong id;
    private bool hasCrown;

    internal ulong Id
    {
        get => id;
        set
        {
            id = value;
        }
    }

    internal bool HasCrown
    {
        get => hasCrown;
        set
        {
            hasCrown = value;
        }
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref id);
        serializer.SerializeValue(ref hasCrown);
    }
}
