using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Character controller for movement in network
/// </summary>
public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private bool usingServerAuthority;
    [SerializeField] private float cheapInterpolationTime = 0.1f;

    private NetworkVariable<PlayerNetworkState> playerState;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        var permission = usingServerAuthority ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        playerState = new NetworkVariable<PlayerNetworkState>(writePerm: permission);
    }

    void Update()
    {
        if (IsOwner)
        {
            TransmitState();
        }
        else
        {
            ConsumeState();
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(transform.GetComponent<PlayerController>());
        }
    }

    private void TransmitState()
    {
        var state = new PlayerNetworkState
        {
            Position = rigidBody.position,
            Rotation = transform.rotation.eulerAngles,
        };

        if (IsServer || !usingServerAuthority)
        {
            playerState.Value = state;
        }
        else
        {
            TransmitStateServerRpc(state);
        }
    }

    [ServerRpc]
    private void TransmitStateServerRpc(PlayerNetworkState state)
    {
        playerState.Value = state;
    }

    private Vector3 posVel;
    private float rotVelY;

    private void ConsumeState()
    {
        //Simple interpolation
        rigidBody.MovePosition(Vector3.SmoothDamp(rigidBody.position, playerState.Value.Position, ref posVel, cheapInterpolationTime));

        transform.rotation = Quaternion.Euler(
            0f,
            Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, playerState.Value.Rotation.y, ref rotVelY, cheapInterpolationTime),
            0f);

    }

    struct PlayerNetworkState : INetworkSerializable
    {
        private float x, z;
        private short yRot;

        internal Vector3 Position
        {
            get => new Vector3(x, 0, z);
            set
            {
                x = value.x;
                z = value.z;
            }
        }

        internal Vector3 Rotation
        {
            get => new Vector3(0, yRot, 0);
            set
            {
                yRot = (short)value.y;
            }
        }


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref x);
            serializer.SerializeValue(ref z);
            serializer.SerializeValue(ref yRot);
        }
    }
}
